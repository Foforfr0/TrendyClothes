using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WpfApp.DTO.User.Auth {
    public class LoginDTO : INotifyPropertyChanged, INotifyDataErrorInfo {
        private string? _username;
        private string? _password;

        private readonly Dictionary<string, List<string>> _errores = new ();

        [Required (ErrorMessage = "El usuario es requerido.")]
        public string? username {
            get => _username;
            set {
                if (_username != value) {
                    _username = value;
                    OnPropertyChanged (nameof (username));
                    ValidarPropiedad (nameof (username), value);
                }
            }
        }

        [Required (ErrorMessage = "La contraseña es requerida.")]
        public string? password {
            get => _password;
            set {
                if (_password != value) {
                    _password = value;
                    OnPropertyChanged (nameof (password));
                    ValidarPropiedad (nameof (password), value);
                }
            }
        }

        private void ValidarPropiedad (string propiedad, object? valor) {
            ValidationContext? contexto = new ValidationContext (this) { MemberName = propiedad };
            List<ValidationResult>? resultados = new List<ValidationResult> ();

            _errores.Remove (propiedad);

            if (!Validator.TryValidateProperty (valor, contexto, resultados)) {
                _errores[propiedad] = resultados.Select (r => r.ErrorMessage!).ToList ();
            }

            ErrorsChanged?.Invoke (this, new DataErrorsChangedEventArgs (propiedad));
            OnPropertyChanged (nameof (HasErrors));
        }

        public IEnumerable GetErrors (string? propertyName) {
            if (string.IsNullOrEmpty (propertyName))
                return _errores.SelectMany (x => x.Value);
            return _errores.ContainsKey (propertyName) ? _errores[propertyName] : Enumerable.Empty<string> ();
        }

        public bool HasErrors => _errores.Any ();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged (string propertyName) {
            PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
        }
    }
}
