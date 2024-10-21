using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using UserRegistry.Models;

namespace UserRegistry.ViewModels
{
    // Crea una classe che eredita da INotifyPropertyChanged e contiene le proprietà che saranno
    // legate alla View.Implementa il metodo PropertyChanged per notificare alla View quando una proprietà cambia.

    internal class UserViewModel : INotifyPropertyChanged
    {
        private User _user = new(); // Inizializza un nuovo oggetto User.
        private User _selectedUser;
        public ObservableCollection<User> Users { get; set; } = []; // Inizializza una nuova collezione di utenti.
        public event PropertyChangedEventHandler PropertyChanged; // Evento che notifica alla View quando una proprietà cambia.

        // Crea una proprietà per l'oggetto User e implementa i metodi get e set per notificare alla View
        public User GetUser 
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Surname));
                OnPropertyChanged(nameof(DateOfBirth));
                OnPropertyChanged(nameof(Address));
                OnPropertyChanged(nameof(City));
                OnPropertyChanged(nameof(Cap));
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                if (_selectedUser != null)
                {
                    GetUser = new User
                    {
                        Name = _selectedUser.Name,
                        Surname = _selectedUser.Surname,
                        DateOfBirth = _selectedUser.DateOfBirth,
                        Address = _selectedUser.Address,
                        City = _selectedUser.City,
                        Cap = _selectedUser.Cap,
                        PhoneNumber = _selectedUser.PhoneNumber
                    };
                }
                OnPropertyChanged(nameof(_selectedUser));
            }
        }

        // Crea una proprietà per ogni campo dell'oggetto User e implementa i metodi get e set per notificare alla View
        public string Name
        {
            get => _user.Name;
            set
            {
                _user.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Surname
        {
            get => _user.Surname;
            set
            {
                _user.Surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        public DateTimeOffset DateOfBirth
        {
            get => _user.DateOfBirth;
            set
            {
                _user.DateOfBirth = value;
                OnPropertyChanged(nameof(DateOfBirth));
            }
        }

        public string Address
        {
            get => _user.Address;
            set
            {
                _user.Address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        public string City
        {
            get => _user.City;
            set
            {
                _user.City = value;
                OnPropertyChanged(nameof(City));
            }
        }

        public int Cap
        {
            get => _user.Cap;
            set
            {
                _user.Cap = value;
                OnPropertyChanged(nameof(Cap));
            }
        }

        public string PhoneNumber
        {
            get => _user.PhoneNumber;
            set
            {
                _user.PhoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public void UpdateSelectedUser()
        {
            if (_selectedUser != null)
            {
                int index = Users.IndexOf(_selectedUser);
                if (index != -1)
                {
                    Users[index] = GetUser;
                }
            }
        }

        // Metodo che notifica alla View che una proprietà è stata modificata.
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}