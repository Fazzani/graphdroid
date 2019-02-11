# GraphQL POC DROID

## To explore

- [x] Relay => Pagination/Cursor/Connection
- [x] Using the dataloader
- [x] Validation des queries
- [x] Filter
  - [ ] Nested queries
- [ ] Multi Sort
- [ ] Batching and caching (DataLoader)
- [ ] Tests unitaires/Tests d'intégration
- [x] Authentication/[Authorization][Graph_authorization] => [UserContext][UserContext]
- [ ] Perf/[Metrics][Metrics]
- [ ] POCer AWS AppSync
- [ ] [Clients side][graphqlclientside] graphql or [Rest GraphQL wrapper][restgraphqlwrapper] approach
- [ ] Comparing the different GraphQL clients [Clients][graphqlclients]
- [ ] GraphQL [code generator][codegenerator]

## Have to see

- [Field middleware](https://graphql-dotnet.github.io/docs/getting-started/field-middleware)
- [Ef with GraphQL](https://hackernoon.com/how-to-implement-generic-queries-by-combining-entityframework-core-and-graphql-net-77ac8faf4a22)
- [GraphQL dotnet Conventions](https://medium.com/@whuysentruit/from-query-to-mutation-with-graphql-conventions-and-asp-net-core-87845f0a2fbd)
- [Awesome tools and extensions for graphql](https://nordicapis.com/10-awesome-tools-and-extensions-for-graphql-apis/)

## Queries examples

```json
{
  __schema {
    types {
      humans
    }
  }
}

query listHumans {
  human {
    id name
  }
}

query GetDroid($id: ID = "1ae34c3b-c1a0-4b7b-9375-c5a221d49e68") {
  droid(id: $id) {
    name
    friends @include(if: $withFriends) {
      name
    }
  }
}

mutation CreateHuman($humanInput: HumanInput!) {
  createHuman(human: $humanInput) {
    name
  }
}

# pagination

{
  humans(first: 1) {
    totalCount
    pageInfo {
      endCursor
      startCursor
    }
    edges {
      node {
        homePlanet
        name
      }
    }
  }
}

# Filter using [System.Linq.Dynamic.Core](https://github.com/StefH/System.Linq.Dynamic.Core)

{
  humans(
    first: 2
    filter: "name.ToLower().Contains(\"dar\") && name.Length > 2"
  ) {
    totalCount
    pageInfo {
      endCursor
      startCursor
    }
    edges {
      node {
        homePlanet
        name
        friends {
          name
          friends {
            appearsIn
            name
          }
        }
      }
    }
  }
}

query droit_pagination($first:Int){
  droids(first: $first, filter: "name.ToLower().Contains(\"d\") && name.Length > 2") {
    totalCount
    pageInfo {
      endCursor
      startCursor
    }
    edges {
      node {
        id
        name
      }
    }
  }
}
# variables
{
  "humanInput": {"name": "test","homePlanet": "Terre"},
  "id": "1ae34c3b-c1a0-4b7b-9375-c5a221d49e68",
  "withFriends": false,
  "first": 2
}
```

## Advantages

- Client First:
  - over fetching: est le surplus d'informations délivrées par la requête par rapport à la donnée désirée par le client.
  - under fetching: est le fait de devoir faire plusieurs appels à l'API pour compléter la réponse de notre premier appel qui ne contient pas assez d'informations
- automatic introspection et documentation

## Authentication && authorization

- On ce qui concerne l'authentifications et l'authorisations, elles sont gérées au même que la validation des requêtes. [example][example_auth]

## References

- [Official Documentation](https://graphql.org/learn)
- [howtographql.com](https://www.howtographql.com)
- [graphql fragments](https://medium.com/graphql-mastery/graphql-fragments-and-how-to-use-them-8ee30b44f59e)

[Graph_authorization]:https://graphql-dotnet.github.io/docs/getting-started/authorization/
[UserContext]:https://graphql-dotnet.github.io/docs/getting-started/user-context
[Metrics]:https://graphql-dotnet.github.io/docs/getting-started/metrics
[graphqlclients]:https://medium.com/open-graphql/exploring-different-graphql-clients-d1bc69de305f
[graphqlclientside]:https://blog.hasura.io/client-side-graphql-schema-resolving-and-schema-stitching-f4d8bccc42d2
[restgraphqlwrapper]:https://graphql.org/blog/rest-api-graphql-wrapper/
[codegenerator]:https://medium.com/the-guild/graphql-code-generator-a34e3785e6fb
[example_auth]:POC_GraphQL\ValidationRules\RequiresAuthValidationRule.cs