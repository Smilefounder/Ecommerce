using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Kooboo.Commerce.Web.Framework.Mvc.ViewEngines
{
    public class CommerceRazorViewEngine : RazorViewEngine
    {
        private const string CacheKeyFormat = ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}:";
        private const string CacheKeyPrefixPartial = "Partial";
        private static readonly string[] _emptyLocations = new string[0];
        private Func<string, string> GetExtensionThunk = VirtualPathUtility.GetExtension;

        public static string[] CommerceSharedPartialViewLocationFormats { get; set; }

        public CommerceRazorViewEngine()
        {
            CommerceSharedPartialViewLocationFormats = new string[] 
            {
                "~/Areas/Commerce/Views/Shared/{0}.cshtml"
            };
        }

        // If the route is registered in a commerce addin,
        // then find partial views in the addin's shared folder first.
        // If view not found, then find partial views in Commerce area shared folder.
        // If view still not found, simply return to let next view engine search partial view for it. 
        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            var isCommerceAddin = controllerContext.RouteData.DataTokens["commerce-addin"];
            if (isCommerceAddin == null || !(bool)isCommerceAddin)
            {
                return new ViewEngineResult(_emptyLocations);
            }

            string[] searched;
            string controllerName = controllerContext.RouteData.GetRequiredString("controller");
            string partialPath = GetPath(controllerContext, AreaPartialViewLocationFormats, CommerceSharedPartialViewLocationFormats, partialViewName, controllerName, CacheKeyPrefixPartial, useCache, out searched);

            if (String.IsNullOrEmpty(partialPath))
            {
                return new ViewEngineResult(searched);
            }

            return new ViewEngineResult(CreatePartialView(controllerContext, partialPath), this);
        }

        private string GetPath(ControllerContext controllerContext, string[] areaLocations, string[] sharedLocations, string name, string controllerName, string cacheKeyPrefix, bool useCache, out string[] searchedLocations)
        {
            searchedLocations = _emptyLocations;

            if (String.IsNullOrEmpty(name))
            {
                return String.Empty;
            }

            var areaName = AreaHelpers.GetAreaName(controllerContext.RouteData);
            List<ViewLocation> viewLocations = GetViewLocations(sharedLocations, areaLocations);

            bool nameRepresentsPath = IsSpecificPath(name);
            string cacheKey = CreateCacheKey(cacheKeyPrefix, name, (nameRepresentsPath) ? String.Empty : controllerName, areaName);

            if (useCache)
            {
                // Only look at cached display modes that can handle the context.
                IEnumerable<IDisplayMode> possibleDisplayModes = DisplayModeProvider.GetAvailableDisplayModesForContext(controllerContext.HttpContext, controllerContext.DisplayMode);
                foreach (IDisplayMode displayMode in possibleDisplayModes)
                {
                    string cachedLocation = ViewLocationCache.GetViewLocation(controllerContext.HttpContext, AppendDisplayModeToCacheKey(cacheKey, displayMode.DisplayModeId));

                    if (cachedLocation == null)
                    {
                        // If any matching display mode location is not in the cache, fall back to the uncached behavior, which will repopulate all of our caches.
                        return null;
                    }

                    // A non-empty cachedLocation indicates that we have a matching file on disk. Return that result.
                    if (cachedLocation.Length > 0)
                    {
                        if (controllerContext.DisplayMode == null)
                        {
                            controllerContext.DisplayMode = displayMode;
                        }

                        return cachedLocation;
                    }
                    // An empty cachedLocation value indicates that we don't have a matching file on disk. Keep going down the list of possible display modes.
                }

                // GetPath is called again without using the cache.
                return null;
            }
            else
            {
                return nameRepresentsPath
                    ? GetPathFromSpecificName(controllerContext, name, cacheKey, ref searchedLocations)
                    : GetPathFromGeneralName(controllerContext, viewLocations, name, controllerName, areaName, cacheKey, ref searchedLocations);
            }
        }

        private string CreateCacheKey(string prefix, string name, string controllerName, string areaName)
        {
            return String.Format(CultureInfo.InvariantCulture, CacheKeyFormat,
                                 GetType().AssemblyQualifiedName, prefix, name, controllerName, areaName);
        }
        
        static string AppendDisplayModeToCacheKey(string cacheKey, string displayMode)
        {
            // key format is ":ViewCacheEntry:{cacheType}:{prefix}:{name}:{controllerName}:{areaName}:"
            // so append "{displayMode}:" to the key
            return cacheKey + displayMode + ":";
        }

        private string GetPathFromGeneralName(ControllerContext controllerContext, List<ViewLocation> locations, string name, string controllerName, string areaName, string cacheKey, ref string[] searchedLocations)
        {
            string result = String.Empty;
            searchedLocations = new string[locations.Count];

            for (int i = 0; i < locations.Count; i++)
            {
                ViewLocation location = locations[i];
                string virtualPath = location.Format(name, controllerName, areaName);
                DisplayInfo virtualPathDisplayInfo = DisplayModeProvider.GetDisplayInfoForVirtualPath(virtualPath, controllerContext.HttpContext, path => FileExists(controllerContext, path), controllerContext.DisplayMode);

                if (virtualPathDisplayInfo != null)
                {
                    string resolvedVirtualPath = virtualPathDisplayInfo.FilePath;

                    searchedLocations = _emptyLocations;
                    result = resolvedVirtualPath;
                    ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, AppendDisplayModeToCacheKey(cacheKey, virtualPathDisplayInfo.DisplayMode.DisplayModeId), result);

                    if (controllerContext.DisplayMode == null)
                    {
                        controllerContext.DisplayMode = virtualPathDisplayInfo.DisplayMode;
                    }

                    // Populate the cache for all other display modes. We want to cache both file system hits and misses so that we can distinguish
                    // in future requests whether a file's status was evicted from the cache (null value) or if the file doesn't exist (empty string).
                    IEnumerable<IDisplayMode> allDisplayModes = DisplayModeProvider.Modes;
                    foreach (IDisplayMode displayMode in allDisplayModes)
                    {
                        if (displayMode.DisplayModeId != virtualPathDisplayInfo.DisplayMode.DisplayModeId)
                        {
                            DisplayInfo displayInfoToCache = displayMode.GetDisplayInfo(controllerContext.HttpContext, virtualPath, virtualPathExists: path => FileExists(controllerContext, path));

                            string cacheValue = String.Empty;
                            if (displayInfoToCache != null && displayInfoToCache.FilePath != null)
                            {
                                cacheValue = displayInfoToCache.FilePath;
                            }
                            ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, AppendDisplayModeToCacheKey(cacheKey, displayMode.DisplayModeId), cacheValue);
                        }
                    }
                    break;
                }

                searchedLocations[i] = virtualPath;
            }

            return result;
        }

        private static bool IsSpecificPath(string name)
        {
            char c = name[0];
            return (c == '~' || c == '/');
        }

        private bool FilePathIsSupported(string virtualPath)
        {
            if (FileExtensions == null)
            {
                // legacy behavior for custom ViewEngine that might not set the FileExtensions property
                return true;
            }
            else
            {
                // get rid of the '.' because the FileExtensions property expects extensions withouth a dot.
                string extension = GetExtensionThunk(virtualPath).TrimStart('.');
                return FileExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
            }
        }

        static List<ViewLocation> GetViewLocations(string[] viewLocationFormats, string[] areaViewLocationFormats)
        {
            List<ViewLocation> allLocations = new List<ViewLocation>();

            if (areaViewLocationFormats != null)
            {
                foreach (string areaViewLocationFormat in areaViewLocationFormats)
                {
                    allLocations.Add(new AreaAwareViewLocation(areaViewLocationFormat));
                }
            }

            if (viewLocationFormats != null)
            {
                foreach (string viewLocationFormat in viewLocationFormats)
                {
                    allLocations.Add(new ViewLocation(viewLocationFormat));
                }
            }

            return allLocations;
        }

        private string GetPathFromSpecificName(ControllerContext controllerContext, string name, string cacheKey, ref string[] searchedLocations)
        {
            string result = name;

            if (!(FilePathIsSupported(name) && FileExists(controllerContext, name)))
            {
                result = String.Empty;
                searchedLocations = new[] { name };
            }

            ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, result);
            return result;
        }

        private class AreaAwareViewLocation : ViewLocation
        {
            public AreaAwareViewLocation(string virtualPathFormatString)
                : base(virtualPathFormatString)
            {
            }

            public override string Format(string viewName, string controllerName, string areaName)
            {
                return String.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, viewName, controllerName, areaName);
            }
        }

        private class ViewLocation
        {
            protected string _virtualPathFormatString;

            public ViewLocation(string virtualPathFormatString)
            {
                _virtualPathFormatString = virtualPathFormatString;
            }

            public virtual string Format(string viewName, string controllerName, string areaName)
            {
                return String.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, viewName, controllerName);
            }
        }
    }
}
