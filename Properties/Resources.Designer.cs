namespace ToDoListApp.Properties {
    using System;
    
    internal class Resources {
        
        private static System.Resources.ResourceManager resourceMan;
        private static System.Globalization.CultureInfo resourceCulture;
        
        internal Resources() {
        }
        
        internal static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    resourceMan = new System.Resources.ResourceManager("ToDoListApp.Properties.Resources", typeof(Resources).Assembly);
                }
                return resourceMan;
            }
        }
        
        internal static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
    }
}
