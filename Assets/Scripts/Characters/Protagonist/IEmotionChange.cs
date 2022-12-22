namespace Characters.Protagonist
{
    public interface IEmotionChange
    {
        float ValueChange { get; }
        EmotionType EmotionToChange { get; }

    }
}