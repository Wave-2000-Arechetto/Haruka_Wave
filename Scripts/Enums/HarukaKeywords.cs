

using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace Haruka.Scripts.Enums;



[RegisterOwnedCardKeyword(nameof(Exercise), CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
[RegisterOwnedCardKeyword(nameof(Bubble))]
[RegisterOwnedCardKeyword(nameof(Scare))]
[RegisterOwnedCardKeyword(nameof(Inspire))]
[RegisterOwnedCardKeyword(nameof(Calm))]
[RegisterOwnedCardKeyword(nameof(ChannelOnFront))]
[RegisterOwnedCardKeyword(nameof(Findroad))]
[RegisterOwnedCardKeyword(nameof(Chance))]
[RegisterOwnedCardKeyword(nameof(Performance))]
[RegisterOwnedCardKeyword(nameof(IronString))]
[RegisterOwnedCardKeyword(nameof(Wanted))]
[RegisterOwnedCardKeyword(nameof(Tame))]
[RegisterOwnedCardKeyword(nameof(WeiSwift))]
[RegisterOwnedCardKeyword(nameof(Agriculture))]
[RegisterOwnedCardKeyword(nameof(Float))]
[RegisterOwnedCardKeyword(nameof(Mio))]
[RegisterOwnedCardKeyword(nameof(Enhanced))]
[RegisterOwnedCardKeyword(nameof(Custom))]
public class HarukaKeywords
{
    public static readonly string Exercise = ModContentRegistry.GetQualifiedKeywordId("Haruka", nameof(Exercise));
    public static readonly string Bubble = ModContentRegistry.GetQualifiedKeywordId("Haruka", nameof(Bubble));
    public static readonly string Scare = ModContentRegistry.GetQualifiedKeywordId("Haruka", nameof(Scare));
    public static readonly string Inspire = ModContentRegistry.GetQualifiedKeywordId("Haruka", nameof(Inspire));
    public static readonly string Calm = ModContentRegistry.GetQualifiedKeywordId("Haruka", nameof(Calm));
    public static readonly string ChannelOnFront = ModContentRegistry.GetQualifiedKeywordId("Haruka", nameof(ChannelOnFront));
    public static readonly string Mio = ModContentRegistry.GetQualifiedKeywordId("Haruka", nameof(Mio));
    
    public static readonly string Findroad = ModContentRegistry.GetQualifiedKeywordId("Haruka", nameof(Findroad));
    public static readonly string Chance = ModContentRegistry.GetQualifiedKeywordId("Haruka", nameof(Chance));
    public static readonly string Performance = ModContentRegistry.GetQualifiedKeywordId("Haruka", nameof(Performance));
    public static readonly string IronString = ModContentRegistry.GetQualifiedKeywordId("Haruka", nameof(IronString));
    public static readonly string Wanted = ModContentRegistry.GetQualifiedKeywordId("Haruka", nameof(Wanted));
    public static readonly string Tame = ModContentRegistry.GetQualifiedKeywordId("Haruka", nameof(Tame));
    public static readonly string WeiSwift = ModContentRegistry.GetQualifiedKeywordId("Haruka", nameof(WeiSwift));
    public static readonly string Float = ModContentRegistry.GetQualifiedKeywordId("Haruka", nameof(Float));
    public static readonly string Agriculture = ModContentRegistry.GetQualifiedKeywordId("Haruka", nameof(Agriculture));
    public static readonly string AsRain = ModContentRegistry.GetQualifiedKeywordId("Haruka", nameof(AsRain));
    public static readonly string Enhanced = ModContentRegistry.GetQualifiedKeywordId("Haruka", nameof(Enhanced));
    public static readonly string Custom = ModContentRegistry.GetQualifiedKeywordId("Haruka", nameof(Custom));

    public static readonly CardKeyword BubbleKeyword = Bubble.GetModCardKeyword();
    public static readonly CardKeyword ScareKeyword = Scare.GetModCardKeyword();
    public static readonly CardKeyword InspireKeyword = Inspire.GetModCardKeyword();

    public static readonly CardKeyword CalmKeyword = Calm.GetModCardKeyword();
    
    public static readonly CardKeyword FloatKeyword = Float.GetModCardKeyword();

    public static readonly CardKeyword MioKeyword = Mio.GetModCardKeyword();

    public static readonly CardKeyword ChannelOnFrontKeyword = ChannelOnFront.GetModCardKeyword();

    public static readonly CardKeyword AsRainKeyword = AsRain.GetModCardKeyword();

    

}