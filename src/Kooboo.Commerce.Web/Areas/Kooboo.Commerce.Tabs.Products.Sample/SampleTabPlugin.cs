using Kooboo.Commerce.Web.Framework.UI;
using Kooboo.Commerce.Web.Framework.UI.Tabs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.Tabs.Products.Sample
{
    public class SampleTabPlugin : TabPlugin<SampleModel>
    {
        public override string Name
        {
            get
            {
                return "Sample";
            }
        }

        public override IEnumerable<Web.Framework.UI.MvcRoute> ApplyTo
        {
            get
            {
                yield return MvcRoutes.Products.Edit();
            }
        }

        public override void OnLoad(TabLoadContext context)
        {
            context.Model = LoadModelFromFile() ?? new SampleModel();
        }

        public override void OnSubmit(TabSubmitContext<SampleModel> context)
        {
            SaveModelToFile(context.Model);
        }

        private SampleModel LoadModelFromFile()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data\\sample.txt");
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path, Encoding.UTF8);
                if (!String.IsNullOrEmpty(json))
                {
                    return JsonConvert.DeserializeObject<SampleModel>(json);
                }
            }

            return null;
        }

        private void SaveModelToFile(SampleModel model)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data\\sample.txt");
            File.WriteAllText(path, JsonConvert.SerializeObject(model), Encoding.UTF8);
        }
    }
}