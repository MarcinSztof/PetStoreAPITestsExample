Quick overview and some reasoning behind some choices I made here.

1. Regarding choice of framework/pattern: There is not much to choose from with .NET API testing frameworks. Port of Java RestAssured is available, but not very actively maintained
and used. Aside from that it's rather opinionated, which can cause troubles at some point. RestSharp may be helpful, but there is still a lot to do on top of it and we have a strong
dependency on specific http client. I chose to build client for this specific API solely based on standard client, abstracting some logic away for this specific implementation 
(this specific API) so it might be easily reused for client for another API. That's only some basic stuff of course, having a setup to build clients for any given API would need a lot more
things added, that's just a PoC created for this assignment. I think that this approach could help ensure scalability and maintainablility. As for how tests are written, I chose 
to write simple tests in NUnit. If tests would have to serve as the only source of documentation then BDD -> SpecFlow might be used, but if that's not very neccessary, I think better not,
as it means a significant toll on maintainability and ease of creation of new tests.

2. There are two projects, one for client and one for tests, but if some of that would be reused, then extracting ApiClientBase and related structure to separate project would be needed.
Here it's omitted for simplicity, but should be quite easy.

3. I added some TODOs, marking places that would need attention when using that for real-life project.

4. Few next things to do here would be:
- Considering creating a wrapper for http client, to drop strong dependency, even on standard one
- If project would grow - some more robust way of managing dependencies, DI container possibly?
- Add logging (here only few symbolic pieces) and persistence for it, maybe using an interceptor to inject logging for all client methods
- Add some ways to autogenerate test data for cases where reasonable coverage of contract validation is too much to achieve by hand
- Few simple issues like more robust handling of paths and queries
- Investigating other possibilities for client structure, maybe composition would be better than inheriting from ApiClientBase?