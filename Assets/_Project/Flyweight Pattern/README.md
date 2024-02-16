# Flyweight Pattern

This example shows you how to handle hundreds of thousands of objects without compromising memory.

The flyweight pattern
already teaches us to share objects that can be used in more than one place to save memory, especially when these
objects contain large pieces of data, like meshes and textures. Fortunately, in Unity and many other modern game
engines, memory management is already taken care of for these heavy objects, so meshes and textures are already being
shared.

Does that mean the flyweight pattern is useless now? Surely not. It can still be applied in cases where you have objects
that store a lot of values and references. Just keep in mind that these are usually less than 8 bytes and the standard
Unity cube is 1.3kB (around 1330 bytes). This means that, unless your object class is REALLY big, you won't benefit much
from it.

At first, I thought of skipping this pattern because it didn't seem so useful in Unity, but after some thinking, I came
uo up with an idea that might be a good example of the pattern. Using Unity ECS (Entity Component System), we're going
to create thousands of objects that share common data, and save a few MB of RAM in the process.

## Explanation

Have you ever noticed that, even if you have 2 copies of a Game Object, the 2 will have different instances of identical
objects? For instance 2 cubes with the standard material have identical Mesh Filter components and identical Mesh
Renderer components. That is most certainly duplicated data.

<p style="text-align: center;"> 2 different cubes, 3 identical components: </p>

![Cubes.png](..%2FCommand%20Pattern%2FImages%2FCubes.png)

Don't get me wrong, this is not a bad thing. This allows us to change a mesh for one object without messing with the
other. But the flyweight pattern is all about reusing objects, so if we have 2 identical cubes that won't change, we
need to share whatever we can between them, event components. And we'll do just that with ECS.

## Implementation

I won't be explaining every detail about how ECS works because we're focusing on the pattern implementation. What you
need to know, in case you already don't is that ECS allows us to create shared components and add them to Entities (the
ECS correspondent of GameObjects).

In this example, we'll create 9 types of pickup coins and use them to create 100,000 entities.

I defined this score system based on the 9 types. It's simple, a coin yields more points the rarer and bigger it is.

![CoinPoints.png](..%2FCommand%20Pattern%2FImages%2FCoinPoints.png)

So the first property that will be shared by the coins are the points. We define a Shared Component in ECS like so:

~~~c#
public struct CoinPoints : ISharedComponentData{
    public int Points;
}
~~~

If you played the game, you might have noticed that there are other properties that are shared between coins, like color
and size. Unlike the Points that need to be accessible to other parts of the code whenever the player collects a coin,
the other properties can be applied when instantiating a coin and discarded later. For instance, the scale can be
applied to the transform, but we won't need it for anything else, so there's no reason to keep it in the shared
component.

**Note**: This is case-specific, if you need to store the initial scale or some other value to access it later on, by
all means, add it to the shared component.

Even though we keep these values just when we're instantiating coins, the Flyweight Pattern can still be used to share
the data to be applied to each new instance. Here we're not really saving any RAM, it's just for better code
readability.

~~~c#
public struct CoinType{
    public float Scale{ get; set; }
    public CoinRotation RotationSpeed{ get; set; }
    public CoinPoints Points { get; set; }
    public CoinColor Color { get; set; }
    
    public CoinType(float scale, float rotationSpeed, int points, float4 color){
        Scale = scale;
        RotationSpeed = new CoinRotation{Value = rotationSpeed};
        Points = new CoinPoints{Points = points};
        Color = new CoinColor{Value = color};
    }
}
~~~

~~~c#
 public partial class GameSystem : SystemBase{
    private static CoinType[] _coinTypes;
    
    private void CreateCoinTypes(Game game){
    _coinTypes = new CoinType[9];
    
    _coinTypes[0] = new CoinType(game.CoinScales.x, game.CoinRotationSpeeds.x, game.CommonCoinPoints.x,
        game.CommonCoinsColor); // Small Common
    _coinTypes[1] = new CoinType(game.CoinScales.y, game.CoinRotationSpeeds.y, game.CommonCoinPoints.y,
        game.CommonCoinsColor); // Normal Common
        
    // All other coin types ...
    }
}
~~~

And then we use use these values when generating a new coin:

~~~c#
 public partial class GameSystem : SystemBase{
    private static CoinType[] _coinTypes;
    
    private void GenerateCoinEntities(Game game){
    // ...
    CoinType coinType = _coinTypes[typeIndex]; // Getting type of coin based on random rareness and size
    
    Entity newEntity = EntityManager.Instantiate(game.CoinPrefab); // Instantiating coin
    
    Vector3 coinPosition = Vector3.zero; // Generating random position
    do{
        coinPosition.x = Random.Range(minMaxX.x, minMaxX.y);
        coinPosition.z = Random.Range(minMaxZ.x, minMaxZ.y);
    } while (Vector3.Distance(coinPosition, position) < 3);
    
    EntityManager.AddComponentData(newEntity, coinType.Color); // Applying material color
    EntityManager.AddComponentData(newEntity, coinType.RotationSpeed); // Applying material rotation
    
    EntityManager.AddSharedComponent(newEntity, coinType.Points); // Adding the Shared Component we created
    
    LocalTransform transform = LocalTransform.FromPosition(coinPosition).ApplyScale(coinType.Scale);
    EntityManager.SetComponentData(newEntity, transform); // Applying scale
    // ...
    }
}
~~~

Then, when the player collides with the coin, we retrieve the shared component and add the points to the score:

~~~c#
private void CollectCoin(CoinPoints coinPoints){
    TotalScore += coinPoints.Points;
}
~~~

# Results

Now, instead of having 100k coins with 100k MonoBehaviours holding the points for each point, we have 100k coins and 6
CoinPoints components that are shared among all those coins. Because this is a really simple example with just one
property (Points), the saved memory is good but not impressive. In a real game scenario you'd probable have many other
things to worry about sharing than just score points for a pickup.

The good news is that, by using ECS we're not only sharing the CoinPoints component, but actually all the components for
mesh rendering are also being shared. This means that all the 100k all also sharing rendering data.

I made some tests to see how much memory we can save by using this approach. You can check the results below. Please
note that I only measured the savings caused by sharing Unity's rendering-related components, without adding any other
components to the coin entities.

<p style="text-align: center;"> Using GameObjects and MonoBehaviours </p>

![ProfilerMonoBehaviour.png](..%2FCommand%20Pattern%2FImages%2FProfilerMonoBehaviour.png)

<p style="text-align: center;"> Using ECS </p>

![ProfilerECS.png](..%2FCommand%20Pattern%2FImages%2FProfilerECS.png)

As we can see, sharing common data is indeed a valuable way to save memory, and I hope this sample helps you to gain
insight of the possibilities the Flyweight Pattern can offer.