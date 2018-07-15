using System;
using Foundation;
using UIKit;
using System.Linq;

namespace DeliveriesA.iOS
{
    public partial class ViewController : UIViewController
    {

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            signinButton.TouchUpInside += SigninButton_TouchUpInside;
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            if (segue.Identifier == "registerSegue")
            {
                var destinationViewController = segue.DestinationViewController as RegisterViewController;
                destinationViewController.emailAddress = emailTextField.Text;
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.		
        }

        private async void SigninButton_TouchUpInside(object sender, EventArgs e)
        {
            var email = emailTextField.Text;
            var password = passwordTextField.Text;
            UIAlertController alertController = null;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                alertController = UIAlertController.Create("InComplete", "Email and password cannot be empty", UIAlertControllerStyle.Alert);
                alertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
            }
            else
            {
                var user = (await AppDelegate.MobileService.GetTable<User>().Where(u => u.Email == email && u.Password == password).ToListAsync()).FirstOrDefault();

                if (user.Password == password)
                {
                    alertController = UIAlertController.Create("Success", "Wellcome", UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create("Thanks", UIAlertActionStyle.Default, null));
                }
                else
                {
                    alertController = UIAlertController.Create("Failure", "Incorrect password", UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                }
            }

            PresentViewController(alertController, true, null);
        }
    }
}
