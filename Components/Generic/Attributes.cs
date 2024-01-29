namespace yt6983138.github.io.Components;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class NotRecommendedAttribute : Attribute
{
	public string? Message { get; }
	public NotRecommendedAttribute(string? message)
	{
		this.Message = message;
	}
}
