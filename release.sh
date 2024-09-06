#!/bin/bash

echo "::set-output name=new_release_version::$1"

# Querier
sed -i "s#<PackageVersion>.*#<PackageVersion>$1</PackageVersion>#" src/Querier/Querier.csproj
sed -i "s#<Version>.*#<Version>$1</Version>#" src/Querier/Querier.csproj

# Querier.SqlQuery
sed -i "s#<PackageVersion>.*#<PackageVersion>$1</PackageVersion>#" src/Querier.SqlQuery/Querier.SqlQuery.csproj
sed -i "s#<Version>.*#<Version>$1</Version>#" src/Querier.SqlQuery/Querier.SqlQuery.csproj