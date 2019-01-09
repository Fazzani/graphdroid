# GraphQL POC DROID

## TODO

- Filter/Search
- Pagination/Cursor/Connection
- Cache
- Using the dataloader

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

# variables
{
  "humanInput": {"name": "test","homePlanet": "Terre"},
  "id": "1ae34c3b-c1a0-4b7b-9375-c5a221d49e68",
  "withFriends": false
}
```