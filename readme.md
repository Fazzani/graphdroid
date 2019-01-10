# GraphQL POC DROID

## Mise en pratique

- [x] Relay => Pagination/Cursor/Connection
- [x] Using the dataloader
- [x] Validation des queries
- [ ] Filter/Search
- [ ] Batching and caching (DataLoader)
- [ ] Tests unitaires/Tests d'intégration
- [ ] Authentication/[Authorization][Graph_authorization]

## Examples

```json
{
  __schema {
    types {
      name
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

query AllHumans(first:1) {
  humans{
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

# variables
{
  "humanInput": {"name": "test","homePlanet": "Terre"},
  "id": "1ae34c3b-c1a0-4b7b-9375-c5a221d49e68",
  "withFriends": false
}
```

## Avantages

- Client First:
  - over fetching: est le surplus d'informations délivrées par la requête par rapport à la donnée désirée par le client.
  - under fetching: est le fait de devoir faire plusieurs appels à l'API pour compléter la réponse de notre premier appel qui ne contient pas assez d'informations
- Introspection et documentation automatique

## References

- [Official Documentation](https://graphql.org/learn)
- [howtographql.com](https://www.howtographql.com)
- [graphql fragments](https://medium.com/graphql-mastery/graphql-fragments-and-how-to-use-them-8ee30b44f59e)

[Graph_authorization]:https://graphql-dotnet.github.io/docs/getting-started/authorization/