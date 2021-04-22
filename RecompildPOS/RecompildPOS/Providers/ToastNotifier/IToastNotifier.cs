using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RecompildPOS.Providers.ToastNotifier
{
    public interface IToastNotifier
    {
        Task<bool> Notify(string title, string description, TimeSpan duration, object context = null, bool showOnTop = true);
        void HideAll();
    }
}
