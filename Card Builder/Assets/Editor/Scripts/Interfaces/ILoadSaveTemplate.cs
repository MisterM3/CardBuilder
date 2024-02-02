namespace CardBuilder
{
    public interface ILoadSaveTemplate<T>
    {

        public void LoadTemplate(TemplateData templateDataToLoad);

        public T SaveTemplate();

    }
}
