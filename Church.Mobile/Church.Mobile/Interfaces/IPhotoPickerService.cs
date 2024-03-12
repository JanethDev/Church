using System.IO;
using System.Threading.Tasks;

namespace Church.Mobile.Interfaces
{
    public interface IPhotoPickerService
    {
        Task<Stream> GetImageStreamAsync();
    }
}
