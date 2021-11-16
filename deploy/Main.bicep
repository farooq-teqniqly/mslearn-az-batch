targetScope = 'subscription'

param solutionName string

@allowed([
  'dev'
  'test'
])
param environmentName string

var location = deployment().location
var baseName = '${solutionName}-${environmentName}-${location}'
var rgName = '${baseName}-rg'

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: rgName
  location: location
}

module batchAccountDeploy 'BatchAccount.bicep' = {
  name: 'batchAccountDeploy'
  scope: rg
  params: {
    baseName: baseName
    location: location
  }
}
