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
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
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

            // container.RegisterType<IConfigSettings, ConfigSettings>(); - will need a new implementations

            container.RegisterType<IScrobblingService, ScrobblingService>();
            container.RegisterType<ILastFmApiClientService, LastFmApiClientService>();
            container.RegisterType<IScrobbler, Scrobbler>();
            container.RegisterType<ICurrentDateTime, CurrentDateTime>();
            container.RegisterType<IPlayProgressTracker, PlayProgressTracker>();

            container.RegisterType<IPlayer, ScrobblingPlayer>(
                new InjectionConstructor(
                    new Player(),
                    container.Resolve<IScrobblingRules>(),
                    container.Resolve<IScrobbler>(),
                    container.Resolve<ICurrentDateTime>(),
                    container.Resolve<IPlayProgressTracker>()
                ));

            var cloudService = container.Resolve<ICloudService>();
            var playlistXmlProvider = new Services.PlaylistXmlProvider(cloudService);
            var trackXmlProvider = new Services.TrackXmlProvider(cloudService);
            
            container.RegisterType<IPlaylistRepository, PlaylistRepository>(
                new InjectionConstructor(container.Resolve<ITrackCriteriaService>(), playlistXmlProvider));

            container.RegisterType<ITrackRepository, TrackRepository>(
                new InjectionConstructor(container.Resolve<ITrackXmlParser>(), trackXmlProvider));

            container.RegisterSingleton<IPlaylist, Playlist>();
        }
    }
}