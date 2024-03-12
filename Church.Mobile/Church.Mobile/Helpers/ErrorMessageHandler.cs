using Church.Mobile.DataLayer.AuxiliaryModels;
using Church.Mobile.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Church.Mobile.Helpers
{
    public static class ErrorMessageHandler
    {
        public static string GetErrorMessage(this Result response)
        {
            string sError = "";
            switch (response)
            {
                case Result.BAD_REQUEST:
                    sError = "Error: Peticion incorrecta.";
                    break;
                case Result.NOT_FOUND:
                    sError = "Error: Recurso no encontrado.";
                    break;
                case Result.INVALID_PASSWORD:
                    sError = Resources.error_invalid_password;
                    break;
                case Result.BLOCKED_USER:
                    sError = "Error: Usuario bloqueado.";
                    break;
                case Result.EXCEPTION:
                case Result.ERROR:
                case Result.ERROR_GETTING_DATA:
                    sError = Resources.error_getting_data;
                    break;
                case Result.SERVICE_EXCEPTION:
                    break;
                case Result.NETWORK_UNAVAILABLE:
                    sError = "Red no disponible.";
                    break;
                default:
                    sError = "Ocurrio un error: (Not handled)";
                    break;
            }
            return sError;
        }
    }
}
