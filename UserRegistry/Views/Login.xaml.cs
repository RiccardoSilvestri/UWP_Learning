using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Diagnostics;
using System;
using UserRegistry.Models;
using Windows.Storage;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using UserRegistry.BLogic;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using Windows.Foundation;
using System.Reflection;
using Windows.UI.Popups;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UserRegistry.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        private List<Credentials> listCredentials = [];
        public Login()
        {
            this.InitializeComponent();
            LoadCredentialsAsync(); // Esecuzione metodo per il caricamento delle credenziali nella
                                    // lista listCredentials. Il metodo è asincrono e viene eseguito
                                    // al caricamento della pagina.
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var view = ApplicationView.GetForCurrentView();
            var bounds = view.VisibleBounds;

            // Dimensioni desiderate
            double desiredWidth = 800;
            double desiredHeight = 600;

            // Calcola la nuova posizione per centrare la finestra
            double x = (bounds.Width - desiredWidth) / 2;
            double y = (bounds.Height - desiredHeight) / 2;

            // Ridimensiona la vista
            bool resizeSuccess = view.TryResizeView(new Size(desiredWidth, desiredHeight));

            if (resizeSuccess)
            {
                // Se il ridimensionamento ha successo, prova a spostare la finestra
                view.TryResizeView(new Size(desiredWidth, desiredHeight));
            }
            else
            {
                // Se il ridimensionamento non riesce, prova a usare le dimensioni correnti
                desiredWidth = bounds.Width;
                desiredHeight = bounds.Height;
            }
        }

        private async void LoginBtn(object sender, RoutedEventArgs e)
        {
            if (chkNewUser.IsChecked == true)
            {
                if (await RegisterNewUser()) // Se nuovo utente registrato con successo, si passa alla pagina di Register.
                    Frame.Navigate(typeof(NavigatorPage), Username.Text);
            }
            else
                CheckCredentials(); // Se utente già registrato, si controllano le credenziali.
        }

        /// <summary>
        /// Il metodo viene chiamato se si verifica un errore durante il login.
        /// </summary>
        /// <param name="sender">E' il controllo da passare al metodo per applicare l'effetto</param>
        /// <returns></returns>
        private static async Task ErrorEffect(Control sender)
        {
            var defaultColor = sender.BorderBrush; // Il metodo salva il colore della pennellata del bordo corrente
                                                   // del controllo sender in una variabile locale defaultColor.
            var defaultTickness = sender.BorderThickness; // Il metodo salva lo spessore della pennellata del bordo corrente
                                                          // del controllo sender in una variabile locale defaultTickness.
            sender.BorderThickness = new Thickness(3); // Aumenta lo spessore della pennellata del bordo del controllo sender.
            sender.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red); // Cambia il colore della pennellata del bordo
                                                                             // del controllo sender in rosso.
            await Task.Delay(2000); // Attende 2 secondi.
            sender.BorderBrush = defaultColor; // Ripristina il colore della pennellata del bordo del controllo sender
                                               // al valore salvato in defaultColor.
            sender.BorderThickness = defaultTickness; // Ripristina lo spessore di default del bordo del controllo sender.
        }

        private async Task<bool> RegisterNewUser()
        {
            bool result = false; // Inizializzazione della variabile result a false.

            try
            {
                string encryptedPassword = EncryptPassword(Password.Password); // Chiamata al metodo EncryptPassword
                                                                               // per criptare la password.
                                                                               // Aggiunta delle credenziali dell'utente appena registrato alla lista listCredentials.
                if (CheckNotExistNewUser(encryptedPassword))
                {
                    listCredentials.Add(new Credentials
                    {
                        Username = Username.Text,
                        Password = encryptedPassword
                    });

                    await FileManager.WriteToJsonFile(listCredentials, "credentials.json", true); //C:\Users\RiccardoSilvestri\AppData\Local\Packages\5a6c4944-6d2b-4aa4-b5a6-a3b733fa2754_xhfdsb7egwnd2\LocalState
                    result = true; // Assegnamento del valore true alla variabile result.
                }
                else 
                    result = false;
            }
            catch (Exception ex) // Gestione delle eccezioni.
            {
                //Gestione eccezione da implementare
            }

            return result;
        }
        private string EncryptPassword(string password)
        {
            string encryptedpwd = string.Empty; // Inizializzazione della variabile encryptedpwd a stringa vuota.

            SHA256 sHA256 = SHA256.Create(); // Creazione di un oggetto di tipo SHA256.
            byte[] bytes = sHA256.ComputeHash(Encoding.UTF8.GetBytes(password)); // Calcolo dell'hash della password
                                                                                 // passata come argomento al metodo.

            // Conversione dell'hash in stringa esadecimale e concatenamento
            for (int i = 0; i < bytes.Length; i++)
            {
                encryptedpwd += bytes[i].ToString("X2"); // Conversione dell'hash in stringa esadecimale e concatenamento
            }

            return encryptedpwd; // Restituzione della password criptata.
        }

        private async void CheckCredentials()
        {

            bool credentialOK = false; // Inizializzazione della variabile credentialOK a false.

            
            string encryptedInputPwd = EncryptPassword(Password.Password);
            await CheckExistLoginUser(encryptedInputPwd);
        }

        private bool CheckNotExistNewUser(string encryptedInputPwd)
        {
            bool rsult = false;

            if (listCredentials.Find
                (usr => usr.Username.ToLower().Equals(Username.Text.ToLower())) == null)
            {
                Debug.WriteLine("Admin logged in at: " + DateTime.Now);
                Console.WriteLine("Admin logged in at: " + DateTime.Now);
                Frame.Navigate(typeof(NavigatorPage), Username.Text);
                rsult = true;
            }
            else
            {
                MessageDialog dialog = new("Utente Già registrato.Mofificare lo Username");
                _ = dialog.ShowAsync();
                rsult = false;
            }

            return rsult;
        }

        private async Task<bool> CheckExistLoginUser(string encryptedInputPwd)
        {
            if (listCredentials.Find
                (usr => usr.Username.ToLower().Equals(Username.Text.ToLower()) &&
                 usr.Password.Equals(encryptedInputPwd)) != null)
            {
                Debug.WriteLine("Admin logged in at: " + DateTime.Now);
                Console.WriteLine("Admin logged in at: " + DateTime.Now);
                Frame.Navigate(typeof(NavigatorPage), Username.Text);
                return false;
            }
            else
            {
                // Applicazione dell'effetto di errore ai controlli Username e Password.
                var usernameErrorEffect = ErrorEffect(Username);
                var pasErrorEffect = ErrorEffect(Password);

                await Task.WhenAll(usernameErrorEffect, pasErrorEffect);
                return true;
            }
        }

        // Metodo chiamato all'avvio della pagina per caricare le credenziali presenti nel file credentials.json
        private async void LoadCredentialsAsync()
        {
            try{
            listCredentials = await FileManager.ReadJsonFile<Credentials>("credentials.json");

            }
            catch(Exception e)
            {
                MessageDialog messageDialog = new("bombazza");

            }
        }
    }
}