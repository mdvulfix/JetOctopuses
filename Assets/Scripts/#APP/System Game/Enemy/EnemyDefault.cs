namespace APP.Game
{
    public class EnemyDefault : EnemyModel<EnemyDefault>, IEnemy
    {
        public override void Configure(params object[] args)
        {
            var config = new EnemyConfig(this);
            base.Configure(config);
        }

    }

}