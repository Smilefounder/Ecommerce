using Kooboo.Commerce.AddIns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Common;
using Kooboo.Commerce.Web.Areas.Commerce.Models.AddIns;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class AddInController : Controller
    {
        private AddInManager _manager = new AddInManager();
        private IAssemblyReferencingService _assemblyReferencingService = AssemblyReferencingServices.Current;
        private IAddInInstaller _installer = new AddInInstaller(AssemblyReferencingServices.Current);
        private IAddInMigrator _migrator = new AddInMigrator(AssemblyReferencingServices.Current);

        public ActionResult Index()
        {
            return View(_manager.GetAllAddInMetas().ToList());
        }

        public ActionResult Install()
        {
            return View(new InstallViewModel());
        }

        [HttpPost]
        public ActionResult Install(string addInId, string @return)
        {
            if (Request.Files.Count > 0)
            {
                var addInFile = Request.Files["AddInFile"];
                var package = new AddInPackage(addInFile.InputStream);
                var errors = package.Validate();

                var model = new InstallViewModel
                {
                    HasUploadedPackage = true
                };

                model.Errors.AddRange(errors.Select(x => x.ErrorMessage));

                if (model.Errors.Count == 0)
                {
                    var currentMeta = _manager.GetAddInMeta(package.Meta.Id);

                    if (currentMeta != null)
                    {
                        if (currentMeta.Version == package.Meta.Version)
                        {
                            model.Errors.Add("Add-in already exists.");
                        }
                        else
                        {
                            model.IsMigration = true;
                            model.CurrentAddInMeta = currentMeta;
                        }
                    }

                    if (model.Errors.Count == 0)
                    {
                        package.Extract(new AddInTempInstallationPath(package.Meta.Id).PhysicalPath);

                        if (model.IsMigration)
                        {
                            var result = _migrator.Prepare(new AddInTempInstallationPath(package.Meta.Id));
                            model.Errors.AddRange(result.Errors);

                            if (result.IsValid)
                            {
                                model.AddInMeta = package.Meta;
                                model.AssemblyConflicts = result.AssemblyConflicts.ToList();
                                model.AssemblyConflictSolutions = GetDefaultAssemblyConflictSolutions(model.AssemblyConflicts);
                            }
                        }
                        else
                        {
                            var result = _installer.Prepare(new AddInTempInstallationPath(package.Meta.Id));
                            model.Errors.AddRange(result.Errors);

                            if (result.IsValid)
                            {
                                model.AddInMeta = package.Meta;
                                model.AssemblyConflicts = result.AssemblyConflicts.ToList();
                                model.AssemblyConflictSolutions = GetDefaultAssemblyConflictSolutions(model.AssemblyConflicts);
                            }
                        }
                    }
                }

                return View(model);
            }
            else
            {
                var data = new JsonResultData(ModelState);
                if (ModelState.IsValid)
                {
                    data.RedirectUrl = Url.Action("InstallConfirm", ControllerContext.RequestContext.AllRouteValues().Merge("AddInId", addInId));
                }

                return Json(data);
            }
        }

        private List<AssemblyConflictSolution> GetDefaultAssemblyConflictSolutions(IEnumerable<AssemblyConflict> conflicts)
        {
            var solutions = new List<AssemblyConflictSolution>();
            foreach (var conflict in conflicts)
            {
                var currentVersion = Version.Parse(conflict.CurrentVersion);
                var newVersion = Version.Parse(conflict.NewVersion);

                if (currentVersion >= newVersion)
                {
                    solutions.Add(new AssemblyConflictSolution(conflict.AssemblyName, false));
                }
                else
                {
                    solutions.Add(new AssemblyConflictSolution(conflict.AssemblyName, true));
                }
            }

            return solutions;
        }

        public ActionResult InstallConfirm(string addInId)
        {
            var path = new AddInTempInstallationPath(addInId);
            var addInMeta = AddInMeta.LoadFrom(PathInfo.Combine(path, AddInMeta.FileName).PhysicalPath);
            return View(addInMeta);
        }

        [HttpPost]
        public ActionResult InstallConfirm(string addInId, FormCollection form)
        {
            var data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                // TODO: Conflict solutions should be posted from client
                var options = new AddInInstallationOptions();
                var addInFolder = new AddInFolder(new AddInTempInstallationPath(addInId));
                var conflicts = _assemblyReferencingService.GetAssemlbyConflicts(addInFolder.GetAssemblies());

                options.AssemblyConflictSolutions = GetDefaultAssemblyConflictSolutions(conflicts);

                var currentMeta = _manager.GetAddInMeta(addInId);
                var isMigration = currentMeta != null;

                // TODO: Think about use an identical base interface for them or so
                if (isMigration)
                {
                    _migrator.DeployFiles(addInId, options);
                    data.RedirectUrl = Url.Action("RunMigration", ControllerContext.RequestContext.AllRouteValues());
                }
                else
                {
                    _installer.DeployFiles(addInId, options);
                    data.RedirectUrl = Url.Action("RunInstallation", ControllerContext.RequestContext.AllRouteValues());
                }
            }

            return Json(data);
        }

        public ActionResult RunInstallation(string addInId)
        {
            _installer.RunInstallation(addInId, new AddInInstallationOptions());
            return Redirect(Url.Action("InstallComplete", ControllerContext.RequestContext.AllRouteValues()));
        }

        public ActionResult RunMigration(string addInId)
        {
            _migrator.RunMigration(addInId, new AddInInstallationOptions());
            return Redirect(Url.Action("InstallComplete", ControllerContext.RequestContext.AllRouteValues()));
        }

        public ActionResult InstallComplete(string addInId, string @return)
        {
            return View();
        }
    }
}
