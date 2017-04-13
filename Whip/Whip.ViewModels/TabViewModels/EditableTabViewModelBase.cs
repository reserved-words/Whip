using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class EditableTabViewModelBase : TabViewModelBase
    {
        private bool _modified;

        public EditableTabViewModelBase(TabType tabType)
            :base(tabType)
        {

        }

        public bool Modified
        {
            get { return _modified; }
            set { Set(ref _modified, value); }
        }

        public virtual void OnSave()
        {
            Modified = false;
        }

        public virtual void OnCancel()
        {
            Modified = false;
        }

        protected void SetModified<T>(ref T property, T value)
        {
            Set(ref property, value);
            Modified = true;
        }
    }
}
