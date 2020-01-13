using System.Threading.Tasks;

namespace RepoToolbox.Utilities
{
    public class SnackbarHandler
    {
        public void ShowMessage(string message, MaterialDesignThemes.Wpf.SnackbarMessageQueue messageQueue) {
            //the message queue can be called from any thread
            Task.Factory.StartNew(() => messageQueue.Enqueue(message));
        }
    }
}
