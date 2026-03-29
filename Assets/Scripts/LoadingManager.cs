using UnityEngine.SceneManagement;

public static class LoadingManager
{
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene
    }
    private static Scene targetScene;

    public static void Load(Scene targetScene)
    {
        LoadingManager.targetScene = targetScene;

        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }
    public static void LoadingCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
