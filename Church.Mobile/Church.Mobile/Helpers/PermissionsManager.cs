using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Church.Mobile.Helpers
{
    public class PermissionsManager
    {
        public async Task<bool> CheckPermissionsAsync(params Permission[] permissions)
        {
            var statuses = await CheckAndRequestPermissionsAsync(permissions);
            foreach (var status in statuses)
            {
                if (status != PermissionStatus.Granted)
                {
                    await Application.Current.MainPage.DisplayAlert("Permisos", "Para brindarle un mejor servicio es necesario que habilite los permisos para esta aplicacion.", "Aceptar");

                    return false;
                }
            }

            return true;
        }

        private async Task<PermissionStatus[]> CheckAndRequestPermissionsAsync(Permission[] permissions)
        {
            var statuses = new Dictionary<Permission, PermissionStatus>();
            foreach (var permission in permissions)
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);

                statuses[permission] = status;
            }

            var notGrandedPermissions = statuses.Where(x => x.Value != PermissionStatus.Granted)
                .Select(x => x.Key)
                .ToArray();

            var results = await CrossPermissions.Current.RequestPermissionsAsync(notGrandedPermissions);

            return results.Values.ToArray();
        }
    }
}