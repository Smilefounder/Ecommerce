using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Configuration;
using Newtonsoft.Json;
using Kooboo.CMS.Common.Runtime.Dependency;

namespace Kooboo.Commerce
{
    [Dependency(typeof(IObjectPersistence), ComponentLifeStyle.Singleton)]
    public class JsonObjectPersistence : IObjectPersistence
    {
        private JsonSerializerSettings _settings;
        private string _rootPath;
        public JsonObjectPersistence()
        {
            _settings = new JsonSerializerSettings();
            _settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            _settings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            string basePath = ConfigurationManager.AppSettings["JsonObjectPersistencePath"];
            if (string.IsNullOrEmpty(basePath))
            {
                basePath = "~/Settings";
            }
            _rootPath = basePath;
        }

        private DirectoryInfo GetDirectory<T>()
        {
            Type type = typeof(T);
            while(type.IsGenericType)
            {
                type = type.GetGenericArguments()[0];
            }

            string path = _rootPath + "/" + type.Name;
            string physicalPath = HttpContext.Current.Server.MapPath(path);
            DirectoryInfo dir = new DirectoryInfo(physicalPath);
            var pd = dir;
            while (!pd.Exists)
            {
                pd.Create();
                pd = pd.Parent;
            }
            return dir;
        }

        private FileInfo GetFile<T>(string name, bool createIfNotExists = false)
        {
            DirectoryInfo dir = GetDirectory<T>();
            var fi = new FileInfo(Path.Combine(dir.FullName, name + ".json"));
            var pd = fi.Directory;
            while(!pd.Exists)
            {
                pd.Create();
                pd = pd.Parent;
            }
            if (!fi.Exists && createIfNotExists)
            {
                var file = new FileInfo(Path.Combine(dir.FullName, name + ".json"));
                if (!file.Exists)
                {
                    var fs = file.Create();
                    fs.Dispose();
                }

                return file;
            }
            
            return fi.Exists ? fi : null;
        }

        public IEnumerable<T> GetAllObjects<T>()
        {
            var dir = GetDirectory<T>();
            var files = dir.GetFiles("*.json", SearchOption.TopDirectoryOnly);
            List<T> objs = new List<T>();
            if (files != null && files.Length > 0)
            {
                foreach (var file in files)
                {
                    string json = File.ReadAllText(file.FullName);
                    if (!string.IsNullOrEmpty(json))
                    {
                        T obj = JsonConvert.DeserializeObject<T>(json, _settings);
                        objs.Add(obj);
                    }
                }
            }
            return objs;
        }

        public T GetObject<T>(string name)
        {
            var file = GetFile<T>(name, false);
            if(file != null)
            {
                string json = File.ReadAllText(file.FullName);
                if (!string.IsNullOrEmpty(json))
                {
                    return JsonConvert.DeserializeObject<T>(json, _settings);
                }
            }
            return default(T);
        }

        public void SaveObject<T>(string name, T obj)
        {
            var file = GetFile<T>(name, true);
            string json = JsonConvert.SerializeObject(obj);
            File.WriteAllText(file.FullName, json);
        }
    }
}
