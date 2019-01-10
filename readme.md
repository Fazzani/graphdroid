# GraphQL POC DROID

## Mise en pratique

- [x] Relay => Pagination/Cursor/Connection
- [x] Using the dataloader
- [x] Validation des queries
- [x] Filter
- [ ] Multi Sort
- [ ] Batching and caching (DataLoader)
- [ ] Tests unitaires/Tests d'intégration
- [ ] Authentication/[Authorization][Graph_authorization]

## TO SEE

- [field middleware](https://graphql-dotnet.github.io/docs/getting-started/field-middleware)
- [Ef with GraphQL](https://hackernoon.com/how-to-implement-generic-queries-by-combining-entityframework-core-and-graphql-net-77ac8faf4a22)

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
  humans(first: 2, filter: "name.ToLower().Contains(\"dar\") && name.Length > 2") {
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

# variables
{
  "humanInput": {"name": "test","homePlanet": "Terre"},
  "id": "1ae34c3b-c1a0-4b7b-9375-c5a221d49e68",
  "withFriends": false
}
```

## Advantages

- Client First:
  - over fetching: est le surplus d'informations délivrées par la requête par rapport à la donnée désirée par le client.
  - under fetching: est le fait de devoir faire plusieurs appels à l'API pour compléter la réponse de notre premier appel qui ne contient pas assez d'informations
- automatic introspection et documentation

## References

- [Official Documentation](https://graphql.org/learn)
- [howtographql.com](https://www.howtographql.com)
- [graphql fragments](https://medium.com/graphql-mastery/graphql-fragments-and-how-to-use-them-8ee30b44f59e)

[Graph_authorization]:https://graphql-dotnet.github.io/docs/getting-started/authorization/