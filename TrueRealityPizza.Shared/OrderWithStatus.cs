using System.Text.Json.Serialization;
using TrueRealityPizza.ComponentsLibrary.Map;

namespace TrueRealityPizza.Shared;

public class OrderWithStatus
{
    public readonly static TimeSpan PreparationDuration = TimeSpan.FromSeconds(10);
    public readonly static TimeSpan DeliveryDuration = TimeSpan.FromMinutes(1); // 虽然不现实，但更有趣

    // 从数据库设置
    public Order Order { get; set; } = null!;

    // 从订单设置
    public string StatusText { get; set; } = null!;

    public bool IsDelivered => StatusText == "已送达";

    public List<Marker> MapMarkers { get; set; } = null!;

    public static OrderWithStatus FromOrder(Order order)
    {
        ArgumentNullException.ThrowIfNull(order.DeliveryLocation);
        // 为了模拟真实的后端处理，我们根据订单下单的时间来伪造状态更新
        string statusText;
        List<Marker> mapMarkers;
        var dispatchTime = order.CreatedTime.Add(PreparationDuration);

        if (DateTime.Now < dispatchTime)
        {
            statusText = "准备中";
            mapMarkers = new List<Marker>
                                {
                                        ToMapMarker("您", order.DeliveryLocation, showPopup: true)
                                };
        }
        else if (DateTime.Now < dispatchTime + DeliveryDuration)
        {
            statusText = "配送中";

            var startPosition = ComputeStartPosition(order);
            var proportionOfDeliveryCompleted = Math.Min(1, (DateTime.Now - dispatchTime).TotalMilliseconds / DeliveryDuration.TotalMilliseconds);
            var driverPosition = LatLong.Interpolate(startPosition, order.DeliveryLocation, proportionOfDeliveryCompleted);
            mapMarkers = new List<Marker>
                                {
                                        ToMapMarker("您", order.DeliveryLocation),
                                        ToMapMarker("骑手", driverPosition, showPopup: true),
                                };
        }
        else
        {
            statusText = "已送达";
            mapMarkers = new List<Marker>
                                {
                                        ToMapMarker("已送达", order.DeliveryLocation, showPopup: true),
                                };
        }

        return new OrderWithStatus
        {
            Order = order,
            StatusText = statusText,
            MapMarkers = mapMarkers,
        };
    }

    private static LatLong ComputeStartPosition(Order order)
    {
        ArgumentNullException.ThrowIfNull(order.DeliveryLocation);
        // 基于订单ID的随机但确定的位置
        var rng = new Random(order.OrderId);
        var distance = 0.01 + rng.NextDouble() * 0.02;
        var angle = rng.NextDouble() * Math.PI * 2;
        var offset = (distance * Math.Cos(angle), distance * Math.Sin(angle));
        return new LatLong(order.DeliveryLocation.Latitude + offset.Item1, order.DeliveryLocation.Longitude + offset.Item2);
    }

    static Marker ToMapMarker(string description, LatLong coords, bool showPopup = false)
            => new Marker { Description = description, X = coords.Longitude, Y = coords.Latitude, ShowPopup = showPopup };
}
