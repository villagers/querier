#!/bin/bash

echo "::set-output name=new_release_version::$1"

# Querier
sed -i "s#<PackageVersion>.*#<PackageVersion>$1</PackageVersion>#" Querier/Querier.csproj
sed -i "s#<Version>.*#<Version>$1</Version>#" Querier/Querier.csproj

# Querier.SqlQuery
sed -i "s#<PackageVersion>.*#<PackageVersion>$1</PackageVersion>#" Querier.SqlQuery/Querier.SqlQuery.csproj
sed -i "s#<Version>.*#<Version>$1</Version>#" Querier.SqlQuery/Querier.SqlQuery.csproj