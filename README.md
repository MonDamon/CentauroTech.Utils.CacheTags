# CentauroTech.Utils.CacheTags

CentauroTech.Utils.CacheTags helps you to add cache tags to your response header's and use it to clean up the content cached in your CDN. 

Your CDN must accept your content to be tagged before you can clean up the content by using tags.
So you'll can  mark related content with identical tags to clean up then in the future instead using CPCODE's or URL's 

## How CentauroTech.Utils.CacheTags works

CentauroTech.Utils.CacheTags works based on three classes:

The first one is CachetTaggable attribute. This attribute is used to mark the method as cachataggable and accepts the parameters from current method to be cached as parameters in attibute constructor

The second one is CacheTaggedApiController. This classes inherits ApiController and overrides ExecuteAsync method and, if enabled, adds the parameterized cache as a header.

The last one is CacheTaggedController that extends MVC AsyncController adding AddCacheTagsHeader method and  enables cache tagged headers.

## Formatting  header's output

Default configuration returns header's as a plain text and to use especific formatting you will need provide a function as CacheTagFormatter. This function should receive a IEnumerable<string> as parameters, format the received lists and returns the a IEnumerble<string> formatted as you wish.

## Configuration Settings

You should pay attention to this two settings key.

1. EnabledCacheTag => This key enables\disables cachetags even for methods marked as taggable. The default value is false and you must set it to true for cachetags be enabled.

2. EdgeCacheTag => This key let you use specific values for cachetag header name instead of default "Edge-Cache-Tag" (Akamai specific). This way you can use CentauroTech.Utils.CacheTags with a large range of CDN's that have it enabled.

3. For CacheTaggedApiController  you can use  property AddCacheTag to override EnabledCacheTag for specifics purposes.

## Example

### WebApi

public abstract class BaseApiController : CacheTaggedBaseApiController

[CacheTaggable("id")]
public virtual HttpResponseMessage Get([FromUri] string id)...


### MVC

public class BaseController : CacheTaggedBaseController

AddCacheTagsHeader(HttpContext, new List<string> {  });


## License

MIT © [CentauroTech](https://gitlab.com/Mondamon)
