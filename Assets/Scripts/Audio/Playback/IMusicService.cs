namespace OrderRushKitchen.Audio
{
    public interface IMusicService
    {
        void Play(MusicTrackId trackId);
        void Stop();
    }
}