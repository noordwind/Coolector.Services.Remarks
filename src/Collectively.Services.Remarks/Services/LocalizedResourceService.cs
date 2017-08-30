using System.Threading.Tasks;
using  Collectively.Common.Types;
using Collectively.Services.Remarks.Repositories;
using Serilog;

namespace Collectively.Services.Remarks.Services
{
    public class LocalizedResourceService : ILocalizedResourceService
    {
        private static readonly ILogger Logger = Log.Logger;
        private ILocalizedResourceRepository _localizedResourceRepository;

        public LocalizedResourceService(ILocalizedResourceRepository localizedResourceRepository)
        {
            _localizedResourceRepository = localizedResourceRepository;
        }

        public async Task<Maybe<string>> TranslateAsync(string name, string culture, params object[] args)
        {
            var resource = await _localizedResourceRepository.GetAsync(name.ToLowerInvariant(), culture.ToLowerInvariant());
            if (resource.HasNoValue)
            {
                Logger.Warning($"Localized resource for name: '{name}' and culture: '{culture}' was not found.");
                
                return new Maybe<string>();
            }

            return resource.Value.GetTranslatedText(args);
        }
    }
}