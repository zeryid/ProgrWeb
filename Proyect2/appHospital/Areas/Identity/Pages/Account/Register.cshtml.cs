// Licenciado a la Fundación .NET bajo uno o más acuerdos.
// La Fundación .NET te otorga una licencia para este archivo bajo la licencia MIT.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using appHospital.Utilidades;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace appHospital.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {// Aquí se definen las dependencias necesarias para el modelo de la página de registro
        private readonly SignInManager<appHospital.Models.Usuario> _signInManager;
        private readonly UserManager<appHospital.Models.Usuario> _userManager;
        private readonly IUserStore<appHospital.Models.Usuario> _userStore;
        private readonly IUserEmailStore<appHospital.Models.Usuario> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _rolemanager;

        // Constructor de la clase RegisterModel que toma las dependencias como parámetros
        public RegisterModel(
            UserManager<appHospital.Models.Usuario> userManager,  // Administrador de usuarios
            IUserStore<appHospital.Models.Usuario> userStore,      // Almacenamiento de usuarios
            SignInManager<appHospital.Models.Usuario> signInManager,  // Administrador de inicio de sesión
            ILogger<RegisterModel> logger,                          // Registrador de eventos
            IEmailSender emailSender,
            RoleManager<IdentityRole>rolemanager
            )                               // Enviador de correos electrónicos
        {
            _userManager = userManager;        // Asigna el administrador de usuarios
            _userStore = userStore;            // Asigna el almacenamiento de usuarios
            _emailStore = GetEmailStore();     // Asigna el almacenamiento de correos electrónicos
            _signInManager = signInManager;    // Asigna el administrador de inicio de sesión
            _logger = logger;                  // Asigna el registrador de eventos
            _emailSender = emailSender;        // Asigna el enviador de correos electrónicos
            _rolemanager = rolemanager;
        }


        // Modelo para recopilar datos de entrada del usuario
        [BindProperty]
        public InputModel Input { get; set; }

        // URL de retorno después del registro
        public string ReturnUrl { get; set; }

        // Lista de esquemas de autenticación externos disponibles
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        // Modelo para los datos de entrada del usuario
        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [MaxLength(50)]
            [Required(ErrorMessage = "Ingrese el nombre del rol")]
            [Display(Name = "Nombre del rol")]
            public string? Usuarios { get; set; }

            // Confirmación de la contraseña del usuario
            [DataType(DataType.Password)]
            //[Display(Name = "Confirmar contraseña")]
            //[Compare("Password", ErrorMessage = "La contraseña y la confirmación de la contraseña no coinciden.")]
            [MaxLength(50)]
            [Required(ErrorMessage = "Ingrese el nombre del rol")]
            [Display(Name = "Nombre del rol")]
            public string? Contraseña { get; set; }

            [MaxLength(10)]
            [Required(ErrorMessage = "Ingrese el nombre del rol")]
            [Display(Name = "Nombre del rol")]
            public string? Estado { get; set; }



        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            // Configuración de la URL de retorno y los esquemas de autenticación externos disponibles
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            // Si no se proporciona una URL de retorno, se establece una predeterminada
            returnUrl ??= Url.Content("~/");
            // Se obtienen los esquemas de autenticación externos disponibles
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                // Si los datos del formulario son válidos, se procede con el registro del usuario
                var user = CreateUser();

                user.Usuarios = Input.Usuarios;
                user.Contraseña = Input.Contraseña;
                user.Estado = Input.Estado;
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Contraseña);
                if (result.Succeeded)
                {
                    if (!await _rolemanager.RoleExistsAsync(CNT.Administrador))
                    {
                        await _rolemanager.CreateAsync(new IdentityRole(CNT.Administrador));
                        await _rolemanager.CreateAsync(new IdentityRole(CNT.Cliente));
                        await _rolemanager.CreateAsync(new IdentityRole(CNT.Paciente));
                        await _rolemanager.CreateAsync(new IdentityRole(CNT.Medico));
                    }

                    string rol = Request.Form["radUsarioRole"].ToString();
                    if( rol == CNT.Administrador )
                        await _userManager.AddToRoleAsync(user, CNT.Administrador);
                    if (rol == CNT.Cliente)
                        await _userManager.AddToRoleAsync(user, CNT.Cliente);
                    if (rol == CNT.Paciente)
                        await _userManager.AddToRoleAsync(user, CNT.Paciente);
                    if (rol == CNT.Medico)
                        await _userManager.AddToRoleAsync(user, CNT.Medico);

                    // Si el registro tiene éxito, se envía un correo electrónico de confirmación si es necesario
                    _logger.LogInformation("El usuario ha creado una nueva cuenta con contraseña.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirme su correo electrónico",
                    //    $"Confirme su cuenta haciendo <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clic aquí</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Si algo falla en el proceso de registro, se vuelve a mostrar el formulario con los errores
            return Page();
        }

        private appHospital.Models.Usuario CreateUser()
        {
            try
            {
                // Se crea una instancia de Usuario utilizando el activador
                return Activator.CreateInstance<appHospital.Models.Usuario>();
            }
            catch
            {
                // Si la creación de la instancia falla, se lanza una excepción
                throw new InvalidOperationException($"No se puede crear una instancia de '{nameof(appHospital.Models.Usuario)}'. " +
                    $"Asegúrate de que '{nameof(appHospital.Models.Usuario)}' no sea una clase abstracta y tenga un constructor sin parámetros, o " +
                    $"sobrescribe la página de registro en /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<appHospital.Models.Usuario> GetEmailStore()
        {
            // Se verifica si el administrador de usuarios admite el almacenamiento de correos electrónicos
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("La interfaz de usuario predeterminada requiere un almacén de usuarios con soporte de correo electrónico.");
            }
            // Se devuelve el almacén de correos electrónicos
            return (IUserEmailStore<appHospital.Models.Usuario>)_userStore;
        }
    }
}
