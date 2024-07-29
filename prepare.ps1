param($version, $gitHead, $repoUri, $filename)
$repoUriWithoutKey = $repoUri -replace 'https:\/\/(.*)@','https://'
$xml = [xml](Get-Content $fileName)
$versionPrefixNode = $xml.SelectSingleNode("//Project/PropertyGroup/VersionPrefix");
$versionPrefixNode.InnerText = $version
$repositoryCommit = $xml.SelectSingleNode("//Project/PropertyGroup/RepositoryCommit");
$repositoryCommit.InnerText = $gitHead
$repositoryUrl = $xml.SelectSingleNode("//Project/PropertyGroup/RepositoryUrl");
$repositoryUrl.InnerText = $repoUriWithoutKey
$packageProjectUrl = $xml.SelectSingleNode("//Project/PropertyGroup/PackageProjectUrl");
$packageProjectUrl.InnerText = $repoUriWithoutKey
[System.Xml.Linq.XDocument]::Parse($Xml.OuterXml).ToString() | Out-File $filename
Write-Host "##vso[task.setvariable variable=version;]$version"