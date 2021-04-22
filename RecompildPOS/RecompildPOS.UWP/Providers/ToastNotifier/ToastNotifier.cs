using System;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using RecompildPOS.Providers.ToastNotifier;

[assembly:Xamarin.Forms.Dependency(typeof(ToastNotifier))]
namespace RecompildPOS.UWP.Providers.ToastNotifier
{
    public class ToastNotifier : IToastNotifier  
    {  
        public Task<bool> Notify(string title,string description, TimeSpan duration, object context = null, bool showOnTop = true)  
        {  
            var taskCompletionSource = new TaskCompletionSource<bool>();

            ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;  
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);  
  
            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");  
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(description));  
              
            XmlNodeList toastImageAttributes = toastXml.GetElementsByTagName("image");  
            ((XmlElement)toastImageAttributes[0]).SetAttribute("src", "ms-appx:///Assets/Logo.scale-240.png");  
            ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "logo");  
  
            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");  
            ((XmlElement)toastNode).SetAttribute("duration", "short");  
  
            var toastNavigationUriString = "#/MainPage.xaml?param1=12345";  
            var toastElement = ((XmlElement)toastXml.SelectSingleNode("/toast"));  
            toastElement.SetAttribute("launch", toastNavigationUriString);  
  
            ToastNotification toast = new ToastNotification(toastXml);  
  
            ToastNotificationManager.CreateToastNotifier().Show(toast);
            return taskCompletionSource.Task;
        }

      
        public void HideAll()
        {
        }
    }  
}