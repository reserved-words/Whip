using LastFmApi;
using LastFmApi.Interfaces;
using System;
using Unity;
using Unity.Injection;
using Whip.Azure;
using Whip.Common.Interfaces;
using Whip.Common.TrackSorters;
using Whip.LastFm;
using Whip.Services;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.Services.Singletons;
using Whip.Web.Interfaces;
using Whip.Web.Services;
using Whip.XmlDataAccess;
using Whip.XmlDataAccess.Interfaces;

namespace Whip.Web
{
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              RegisterSingletons(container);
              return container;
          });

        public static IUnityContainer Container => container.Value;
        #endregion

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<ITrackQueue, TrackQueue>();
            container.RegisterType<IDefaultTrackSorter, DefaultTrackSorter>();
            container.RegisterType<IRandomTrackSorter, RandomTrackSorter>();
            container.RegisterType<ITrackCriteriaService, TrackCriteriaService>();
            container.RegisterType<ICloudService, AzureService>();
            container.RegisterType<ITrackXmlParser, TrackXmlParser>();
            container.RegisterType<IPlaylistService, PlaylistService>();
            container.RegisterType<IScrobblingRules, ScrobblingRules>();
            container.RegisterType<ISessionService, SessionService>();
            container.RegisterType<IConfigSettings, Services.ConfigSettings>();
            container.RegisterType<IScrobblingService, ScrobblingService>();
            container.RegisterType<IScrobbler, Scrobbler>();
            container.RegisterType<ICurrentDateTime, CurrentDateTime>();
            container.RegisterType<IPlayProgressTracker, PlayProgressTracker>();

            var cloudService = container.Resolve<ICloudService>();
            var playlistXmlProvider = new Services.PlaylistXmlProvider(cloudService);
            var trackXmlProvider = new Services.TrackXmlProvider(cloudService);
            
            container.RegisterType<IPlaylistRepository, PlaylistRepository>(
                new InjectionConstructor(container.Resolve<ITrackCriteriaService>(), playlistXmlProvider));

            container.RegisterType<ITrackRepository, TrackRepository>(
                new InjectionConstructor(container.Resolve<ITrackXmlParser>(), trackXmlProvider));
        }

        private static void RegisterSingletons(IUnityContainer container)
        {
            container.RegisterSingleton<IPlaylist, Playlist>();
            container.RegisterSingleton<ILastFmApiClientService, LastFmApiClientService>();

            container.RegisterSingleton<IPlayer, ScrobblingPlayer>(
                new InjectionConstructor(
                    new Player(),
                    container.Resolve<IScrobblingRules>(),
                    container.Resolve<IScrobbler>(),
                    container.Resolve<ICurrentDateTime>(),
                    container.Resolve<IPlayProgressTracker>()
                ));
        }
    }
}