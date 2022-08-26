using System.ComponentModel;

namespace Nuget.UnlistAll.Models
{
    /// <summary>
    /// Package version item which is selectable from UI
    /// </summary>
    public class PackageVersion : INotifyPropertyChanged
    {
        public PackageVersion(string packageId, string version )
        {
            this.PackageId = packageId;
            this.Version = version; 
        }
         

        public string PackageId { get;  set; }

        public string Version { get;  set; }

        //public bool Selected
        //{
        //    get { return _selected; }
        //    set
        //    {
        //        if (_selected != value)
        //        {
        //            _selected = value;
        //            OnPropertyChanged(nameof(Selected));
        //        }
        //    }
        //}
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override int GetHashCode()
        {
            return PackageId.GetHashCode()^ Version.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(!(obj is PackageVersion pkg))
            {
                return false;
            }
            return pkg.PackageId == PackageId && pkg.Version == Version;
        }
    }
}
