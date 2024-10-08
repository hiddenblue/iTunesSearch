using iTunesSearch.Library.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
// using System.Diagnostics;
// using System.Linq;

namespace iTunesSearch.Library.Tests
{

    public class iTunesSearchTests
    {
        [Fact]
        public async Task GetArtistById_ValidArtist_ReturnsArtist()
        {
            // Arrange
            iTunesSearchManager search = new iTunesSearchManager();
            long artistId = 311145;

            // Act
            var item = await search.GetSongArtistByArtistIdAsync(artistId);

            // Assert
            Assert.Equal("R.E.M.", item.ArtistName);
        }

        [Fact]
        public async Task GetArtistById_InvalidArtist_ReturnsDefaultInstanceOfSongArtist()
        {
            // Arrange
            iTunesSearchManager search = new iTunesSearchManager();
            long artistId = 5858500001;

            // Act
            var item = await search.GetSongArtistByArtistIdAsync(artistId);

            // Assert
            Assert.IsType<SongArtist>(item);
            Assert.Equal(default(string), item.ArtistName);
        }

        [Fact]
        public async Task GetArtistByAMGArtistId_ValidArtist_ReturnsArtist()
        {
            // Arrange
            iTunesSearchManager search = new iTunesSearchManager();
            long amgArtistId = 116437;

            // Act
            var item = await search.GetSongArtistByAMGArtistIdAsync(amgArtistId);

            // Assert
            Assert.Equal("R.E.M.", item.ArtistName);
        }

        [Fact]
        public async Task GetArtistByAMGArtistId_InvalidArtist_ReturnsDefaultInstanceOfSongArtist()
        {
            // Arrange
            iTunesSearchManager search = new iTunesSearchManager();
            long amgArtistId = 5858500001;

            // Act
            var item = await search.GetSongArtistByAMGArtistIdAsync(amgArtistId);

            // Assert
            Assert.IsType<SongArtist>(item);
            Assert.Equal(default(string), item.ArtistName);
        }

        [Fact]
        public async Task GetAlbumsByArtistId_ValidArtist_ReturnsAlbums()
        {
            // Arrange
            iTunesSearchManager search = new iTunesSearchManager();
            long artistId = 311145;

            // Act
            var items = await search.GetAlbumsByArtistIdAsync(artistId, 200);

            // Assert
            Assert.Contains(items.Albums, (al => al.CollectionName == "Automatic For The People"));
        }

        [Fact]
        public async Task GetAlbumsByAMGArtistId_ValidArtist_ReturnsAlbums()
        {
            // Arrange
            iTunesSearchManager search = new iTunesSearchManager();
            long amgArtistId = 116437;

            // Act
            var items = await search.GetAlbumsByAMGArtistIdAsync(amgArtistId, 200);

            // Assert
            Assert.Contains(items.Albums, (al => al.CollectionName == "Out of Time (2016 Remaster)"));
        }

        [Fact]
        public async Task GetSongArtists_ValidArtists_ReturnsArtists()
        {
            // Arrange
            iTunesSearchManager search = new iTunesSearchManager();
            string artist = "R.E.M.";

            // Act
            var items = await search.GetSongArtistsAsync(artist, 200);

            // Assert
            Assert.True(items.Artists.Any());
        }

        [Fact]
        public async Task GetSongs_ValidSongs_ReturnsSongs()
        {
            // Arrange
            iTunesSearchManager search = new iTunesSearchManager();
            string song = "Driver 8";

            // Act
            var items = await search.GetSongsAsync(song, 200);

            // Assert
            Assert.True(items.Songs.Any());
            Assert.Contains(items.Songs, (s => s.TrackName == "Driver 8" && s.ArtistName == "R.E.M."));
        }

        [Fact]
        public async Task GetAlbums_ValidAlbums_ReturnsAlbums()
        {
            // Arrange
            iTunesSearchManager search = new iTunesSearchManager();
            string album = "Collapse into Now";

            // Act
            var items = await search.GetAlbumsAsync(album, 200);

            // Assert
            Assert.True(items.Albums.Any());
            Assert.Contains(items.Albums, (al => al.ArtistName == "R.E.M."));
        }

        [Fact]
        public async Task GetTVEpisodesForShow_ValidShow_ReturnsEpisodes()
        {
            //  Arrange
            iTunesSearchManager search = new iTunesSearchManager();
            string showName = "Modern Family";

            //  Act
            var items = await search.GetTVEpisodesForShow(showName, 200);

            //  Assert
            Assert.True(items.Episodes.Any());
        }

        [Fact]
        public async Task GetTVEpisodesForShow_ValidShow_GroupedEpisodes()
        {
            //  Arrange
            iTunesSearchManager search = new iTunesSearchManager();
            string showName = "Modern Family";

            //  Act
            var items = await search.GetTVEpisodesForShow(showName, 200);
            var seasons = from episode in items.Episodes
                          orderby episode.Number
                          group episode by episode.SeasonNumber into seasonGroup
                          orderby seasonGroup.Key
                          select seasonGroup;

            //  Assert
            foreach (var seasonGroup in seasons)
            {
                Debug.WriteLine("Season number: {0}", seasonGroup.Key);

                foreach (TVEpisode episode in seasonGroup)
                {
                    Debug.WriteLine("Ep {0}: {1}", episode.Number, episode.Name);
                }
            }
        }

        [Fact]
        public async Task GetTVSeasonsForShow_ValidShow_ReturnsShows()
        {
            //  Arrange
            iTunesSearchManager search = new iTunesSearchManager();
            string showName = "Modern Family";

            //  Act
            var items = await search.GetTVSeasonsForShow(showName);

            //  Assert
            Assert.True(items.Seasons.Any());
        }

        [Fact]
        public async Task GetTVSeasonById_ValidSeasonId_ReturnsShow()
        {
            //  Arrange
            iTunesSearchManager search = new iTunesSearchManager();
            long seasonId = 327827178;

            //  Act
            var items = await search.GetTVSeasonById(seasonId);

            //  Assert
            Assert.True(items.Seasons.Any());
            Assert.Equal("Modern Family", items.Seasons.First().ShowName);
        }

        [Fact]
        public async Task GetTVSeasonById_ValidSeasonId_ReturnsCorrectLargeArtwork()
        {
            //  Arrange
            iTunesSearchManager search = new iTunesSearchManager();
            long seasonId = 316075588;
            string expectedShowName = "Gilmore Girls";
            string expectedLargeArtworkUrl = "https://is1-ssl.mzstatic.com/image/thumb/Video71/v4/37/76/85/37768544-2dd0-6473-b2db-97a5f777a551/mzl.jfghamax.lsr/600x600bb.jpg";

            //  Act
            var items = await search.GetTVSeasonById(seasonId);

            //  Assert
            Assert.True(items.Seasons.Any());
            Assert.Equal(expectedShowName, items.Seasons.First().ShowName);
            Assert.Equal(expectedLargeArtworkUrl, items.Seasons.First().ArtworkUrlLarge);
        }

        [Fact]
        public async Task GetTVSeasonsForShow_ValidShowAndCountryCode_ReturnsShows()
        {
            //  Arrange
            iTunesSearchManager search = new iTunesSearchManager();
            string showName = "Top of the Lake";
            string countryCode = "AU"; /* Australia */

            //  Act
            var items = await search.GetTVSeasonsForShow(showName, 20, countryCode);

            //  Assert
            Assert.True(items.Seasons.Any());
        }

        [Fact]
        public async Task GetTVEpisodesForShow_ValidShowAndCountryCode_ReturnsEpisodes()
        {
            //  Arrange
            iTunesSearchManager search = new iTunesSearchManager();
            string showName = "Top of the Lake";
            string countryCode = "AU"; /* Australia */

            //  Act
            var items = await search.GetTVEpisodesForShow(showName, 200, countryCode);

            //  Assert
            Assert.True(items.Episodes.Any());
        }

        [Fact]
        public async Task GetPodcasts_ValidPodcast_ReturnsEpisodes()
        {
            //  Arrange
            iTunesSearchManager search = new iTunesSearchManager();
            string showName = "Radiolab";

            //  Act
            var items = await search.GetPodcasts(showName, 200);

            //  Assert
            Assert.True(items.Podcasts.Any());
        }

        [Fact]
        public async Task GetPodcastById_ValidId_ReturnsPodcast()
        {
            //  Arrange
            iTunesSearchManager search = new iTunesSearchManager();
            long podcastId = 1002937870;

            //  Act
            var items = await search.GetPodcastById(podcastId);

            //  Assert
            Assert.True(items.Podcasts.Any());
            Assert.Equal("Dear Hank & John", items.Podcasts.First().Name);
            Assert.Equal("https://feeds.simplecast.com/9YNI3WaL", items.Podcasts.First().FeedUrl);
        }
    }
}