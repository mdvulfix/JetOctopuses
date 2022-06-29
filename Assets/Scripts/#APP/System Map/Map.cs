namespace APP.Map
{
    public class MapModel<TMap>
    {
        protected int m_Hight;
        protected int m_Width;


    }

    public class MapDefault: MapModel<MapDefault>
    {
        public MapDefault()
        {
            m_Hight = 1480;
            m_Width = 720;
        }

        public MapDefault(int hight, int width)
        {
            m_Hight = hight;
            m_Width = width;
        }

    }

}