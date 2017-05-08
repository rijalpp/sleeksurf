using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Web;

namespace SleekSurf.FrameWork
{
    public static class MEFManager
    {
        public static void Compose(object obj)
        {
            try
            {
                var catalog = new AggregateCatalog();
                catalog.Catalogs.Add(new DirectoryCatalog(@".\bin"));
                var container = new CompositionContainer(catalog);
                container.ComposeParts(obj);
            }
            catch (ReflectionTypeLoadException tLException)
            {
                Helpers.LogError(tLException);
                var loaderMessages = new StringBuilder();
                loaderMessages.AppendLine("While trying to load composable parts the follwing loader exceptions were found: ");
                foreach (var loaderException in tLException.LoaderExceptions)
                {
                    loaderMessages.AppendLine(loaderException.Message);
                }

                // this is one of our custom exception types.   
                //throw new PluginLoadingException(loaderMessages.ToString(), tLException);

                string result = loaderMessages.ToString();
            }

        }
    }
}
