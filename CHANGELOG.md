## [2.1.0](https://github.com/villagers/querier/compare/v2.0.0...v2.1.0) (2024-10-08)

### Features

* add `QueryJoin` attribute ([c6f4a82](https://github.com/villagers/querier/commit/c6f4a822c92358dd555ed54146ff895fb83a4b78))

## [2.0.0](https://github.com/villagers/querier/compare/v1.7.2...v2.0.0) (2024-10-05)

### ⚠ BREAKING CHANGES

* 

### Features

* a better fluent query filter ([3aeba1a](https://github.com/villagers/querier/commit/3aeba1ac079c017fe193886ac066271b0affd1ff))
* allow `Raw` querier, add `Join` methods, add `Case` methods ([b2de599](https://github.com/villagers/querier/commit/b2de599441ad6027d21854cbc31c6aa90a48bb8a))
* allow `union` queries ([9a2705a](https://github.com/villagers/querier/commit/9a2705a8200e31daef86600329384a3532e216bf))
* meta attributes, enable switch, initialization check ([bd68c13](https://github.com/villagers/querier/commit/bd68c13ab3125c7971f0dc14609453f7d39dff1c))
* version  2 ([b82dcaa](https://github.com/villagers/querier/commit/b82dcaa3cd70d441d1be529cf2961a4bd5342171))

### Bug Fixes

* calling `QuerySchemaInitiator` multiple times ([eba5348](https://github.com/villagers/querier/commit/eba53484aa82ed4f485bc749380ace5336e3c989))
* construct `SqlQueryResult` ([23c5773](https://github.com/villagers/querier/commit/23c57731fd544f95cf1a643770c2cb7198346b8c))
* ensure data source file has a `.db` extensions ([68f0106](https://github.com/villagers/querier/commit/68f010612a4b05c930eee18019d5e193242a5d1d))
* group by column order ([b6effd9](https://github.com/villagers/querier/commit/b6effd9758528f39df55b874f917bc7b282f9041))
* group by sql dimensions and time dimensions ([89dc53e](https://github.com/villagers/querier/commit/89dc53e3adcb3f477b325903e636a9aec9c98536))
* remove extra parameters ([283773d](https://github.com/villagers/querier/commit/283773df1d0c10d07544415ae0cfc1e4ff5c63dd))
* remove redundant `.ToString()` ([3529b8c](https://github.com/villagers/querier/commit/3529b8c90844ad323fc6023af99aa9045ce13051))
* run schedulers on the same worker ([d148ed8](https://github.com/villagers/querier/commit/d148ed87250dcb044560d54580d113ce6bb6899b))
* tests ([88625b4](https://github.com/villagers/querier/commit/88625b4d815092e9014dc9dbff3746dc3d40b31b))
* update `appsettings.ci.json` ([0c6cbdf](https://github.com/villagers/querier/commit/0c6cbdf7b27616b2dd160ded24eb99fb0dbb0af3))
* use `AppendNullValue` when value is `DBNull` ([8dffffb](https://github.com/villagers/querier/commit/8dffffb57dbc011be6eaccae7df6e89e5f5acc45))
* use `AppendNullValue` when value type is `DBNull` ([e43a2dd](https://github.com/villagers/querier/commit/e43a2ddcd7fd5bd40b90092a4421297fbc9a036f))
* use multiple connections ([8f6023b](https://github.com/villagers/querier/commit/8f6023bba88c82eae6186b4ff318fec291970dc8))

### Miscellaneous Chores

* add other database files ([62b45a1](https://github.com/villagers/querier/commit/62b45a1af77d4d090f356fdf6902fcfb40f70891))
* add other database providers ([d7ed590](https://github.com/villagers/querier/commit/d7ed5905a47ffc2b3b2b03bd0d2aaab882095de6))
* change mysql tests namespace ([3072d88](https://github.com/villagers/querier/commit/3072d8836d1ac55e82b976d812317de50bf59d1b))
* **deps:** update dependency coravel to v6 ([c16e0c5](https://github.com/villagers/querier/commit/c16e0c5668aa33ea48d1acd5bc08b43c8d7009e5))
* **deps:** update dependency coverlet.collector to 6.0.2 ([1ba5a36](https://github.com/villagers/querier/commit/1ba5a3695c76482b510205e8985f16466583ce9f))
* **deps:** update dependency microsoft.net.test.sdk to 17.11.1 ([94b9d10](https://github.com/villagers/querier/commit/94b9d104ec2cf10935a71f062387ed60d0e09e21))
* **deps:** update dependency npgsql to 8.0.4 ([126ca35](https://github.com/villagers/querier/commit/126ca3522284b8a9669a6b61d983f6be8fd9ef29))
* **deps:** update dependency oracle.manageddataaccess.core to 23.6.0 ([f0bad9c](https://github.com/villagers/querier/commit/f0bad9c15332ce692c1dc9c76759c6a41a1bc8c1))
* **deps:** update dependency swashbuckle.aspnetcore to 6.8.1 ([2488dc9](https://github.com/villagers/querier/commit/2488dc9cc07d8b3c872004a59b1489f3908bf689))
* **deps:** update dependency system.data.sqlite.core to 1.0.119 ([cd1fe86](https://github.com/villagers/querier/commit/cd1fe861da81029e026e77055caae9bdcb1c293e))
* **deps:** update dependency xunit to 2.9.2 ([c9b2eb6](https://github.com/villagers/querier/commit/c9b2eb67a92bc7ee7cd0b009366cd5c52e71922a))
* **deps:** update xunit-dotnet monorepo ([7817a93](https://github.com/villagers/querier/commit/7817a9395face428c0bd6acd626f8d7fc1879b10))
* **release:** 2.0.0-beta.1 [skip ci] ([4c78b08](https://github.com/villagers/querier/commit/4c78b08ed543ec463d4cbc0413935a7748abc64e))
* **release:** 2.0.0-beta.2 [skip ci] ([7311bcc](https://github.com/villagers/querier/commit/7311bcc134c8e52e68d3c80abb226f0cf9302f49))
* **release:** 2.0.0-beta.3 [skip ci] ([ffa37e9](https://github.com/villagers/querier/commit/ffa37e9c52013087073bb9832062c64c774f090f))
* **release:** 2.0.0-beta.4 [skip ci] ([5e68d2d](https://github.com/villagers/querier/commit/5e68d2decaf24192165ac6f99ad69ccc9d611980))
* **release:** 2.0.0-beta.5 [skip ci] ([69db482](https://github.com/villagers/querier/commit/69db482f7b5ebfa397f0d138194eaf1e84e8988c))
* **release:** 2.0.0-beta.6 [skip ci] ([3ccd32b](https://github.com/villagers/querier/commit/3ccd32b486add87d5c07ce82825f24901576887d))
* **release:** 2.0.0-beta.7 [skip ci] ([585da54](https://github.com/villagers/querier/commit/585da54c09442b279962a2c9f873b2e0a450c46a))
* **release:** 2.0.0-beta.8 [skip ci] ([4930cc6](https://github.com/villagers/querier/commit/4930cc60394985b00f600aca82e57df868dd2c7c))
* **release:** add `conventional-changelog-conventionalcommits` plugin ([5c719b7](https://github.com/villagers/querier/commit/5c719b721069a2b06c60e05342deca304d6a53bc))
* remove `using` ([bb73c3f](https://github.com/villagers/querier/commit/bb73c3f9fd46afb118a4b0462d4ac6dac0ee78f2))
* run builds on push ([8ba1cb6](https://github.com/villagers/querier/commit/8ba1cb6ae939db429e030949f00a6b59a61d6aa9))
* run tests on push ([1481e4e](https://github.com/villagers/querier/commit/1481e4e10756213a12bf8523e8ac3d8700a0635d))
* update `.releaserc` ([c10a76a](https://github.com/villagers/querier/commit/c10a76a483feb7454867031d6c907421ab87a042))
* **workflow:**  update mysql database file name ([b01e4dc](https://github.com/villagers/querier/commit/b01e4dc29b8c79da234a66c12bcc4badbe62c6c7))

# [2.0.0-beta.8](https://github.com/villagers/querier/compare/v2.0.0-beta.7...v2.0.0-beta.8) (2024-10-02)


### Bug Fixes

* run schedulers on the same worker ([954863f](https://github.com/villagers/querier/commit/954863f40e698b0f83fe0fc7d30319eb7ed236c8))

# [2.0.0-beta.7](https://github.com/villagers/querier/compare/v2.0.0-beta.6...v2.0.0-beta.7) (2024-10-01)


### Bug Fixes

* construct `SqlQueryResult` ([1f6c0e7](https://github.com/villagers/querier/commit/1f6c0e7519006a1a3ebed2c59083f866e1f67a28))

# [2.0.0-beta.6](https://github.com/villagers/querier/compare/v2.0.0-beta.5...v2.0.0-beta.6) (2024-10-01)


### Bug Fixes

* remove redundant `.ToString()` ([172dd38](https://github.com/villagers/querier/commit/172dd3833383e0866aa10ce1a3cb7cc4f366456a))

# [2.0.0-beta.5](https://github.com/villagers/querier/compare/v2.0.0-beta.4...v2.0.0-beta.5) (2024-10-01)


### Bug Fixes

* calling `QuerySchemaInitiator` multiple times ([74c8cf9](https://github.com/villagers/querier/commit/74c8cf917fdc0734d6e4fb3ceeae9bc9c34361c5))

# [2.0.0-beta.4](https://github.com/villagers/querier/compare/v2.0.0-beta.3...v2.0.0-beta.4) (2024-10-01)


### Features

* meta attributes, enable switch, initialization check ([5c3c74f](https://github.com/villagers/querier/commit/5c3c74f2329adde08daf8edf4cb6bccc3c3523ec))

# [2.0.0-beta.3](https://github.com/villagers/querier/compare/v2.0.0-beta.2...v2.0.0-beta.3) (2024-09-28)


### Bug Fixes

* group by column order ([5ffa45e](https://github.com/villagers/querier/commit/5ffa45e5b2464b406079be0b43436cbfd5580294))

# [2.0.0-beta.2](https://github.com/villagers/querier/compare/v2.0.0-beta.1...v2.0.0-beta.2) (2024-09-28)


### Bug Fixes

* ensure data source file has a `.db` extensions ([d0e8e34](https://github.com/villagers/querier/commit/d0e8e34d9eceb2419f6a7060300271903e6e6cb6))
* group by sql dimensions and time dimensions ([903375d](https://github.com/villagers/querier/commit/903375d0c32f114a06bf4e638f190b8ec87ea34a))
* remove extra parameters ([2a6d832](https://github.com/villagers/querier/commit/2a6d83241915e80734055c6a5ac1c9f49342da3b))
* update `appsettings.ci.json` ([9554d98](https://github.com/villagers/querier/commit/9554d98021122228f206f5c91e9afcf5115a8de2))
* use `AppendNullValue` when value is `DBNull` ([da3c756](https://github.com/villagers/querier/commit/da3c75636906975f5af1ecc9ee012a66f11cacf2))
* use `AppendNullValue` when value type is `DBNull` ([58bbaad](https://github.com/villagers/querier/commit/58bbaadec515684be5a84c6d74a4d31a0eb40074))

# [2.0.0-beta.1](https://github.com/villagers/querier/compare/v1.7.2...v2.0.0-beta.1) (2024-09-28)


### Features

* allow `Raw` querier, add `Join` methods, add `Case` methods ([b2de599](https://github.com/villagers/querier/commit/b2de599441ad6027d21854cbc31c6aa90a48bb8a))
* allow `union` queries ([9a2705a](https://github.com/villagers/querier/commit/9a2705a8200e31daef86600329384a3532e216bf))
* version  2 ([f56fcdf](https://github.com/villagers/querier/commit/f56fcdfd322655a2a1cda79dc98ae8f07ee39160))


### BREAKING CHANGES

* Breaking changes to the API

# [1.8.0-beta.1](https://github.com/villagers/querier/compare/v1.7.2...v1.8.0-beta.1) (2024-09-28)


### Features

* allow `Raw` querier, add `Join` methods, add `Case` methods ([b2de599](https://github.com/villagers/querier/commit/b2de599441ad6027d21854cbc31c6aa90a48bb8a))
* allow `union` queries ([9a2705a](https://github.com/villagers/querier/commit/9a2705a8200e31daef86600329384a3532e216bf))

# [1.8.0-beta.1](https://github.com/villagers/querier/compare/v1.7.2...v1.8.0-beta.1) (2024-09-28)


### Features

* allow `Raw` querier, add `Join` methods, add `Case` methods ([b2de599](https://github.com/villagers/querier/commit/b2de599441ad6027d21854cbc31c6aa90a48bb8a))
* allow `union` queries ([9a2705a](https://github.com/villagers/querier/commit/9a2705a8200e31daef86600329384a3532e216bf))

## [1.7.2](https://github.com/villagers/querier/compare/v1.7.1...v1.7.2) (2024-09-10)


### Bug Fixes

* remove orderby ([420c4bb](https://github.com/villagers/querier/commit/420c4bbd2ef27f1a8ad53cf394ad9e2d29cd6747))

## [1.7.1](https://github.com/villagers/querier/compare/v1.7.0...v1.7.1) (2024-09-09)


### Bug Fixes

* improvements ([bae94f1](https://github.com/villagers/querier/commit/bae94f16406a9f46bbfc79e220685a9005639308))

# [1.7.0](https://github.com/villagers/querier/compare/v1.6.2...v1.7.0) (2024-09-08)


### Features

* add default `Measure` method point to `MeasureSum` ([aba13d3](https://github.com/villagers/querier/commit/aba13d3764eaedf991fac7885acd4f2b0a2dad34))

## [1.6.2](https://github.com/villagers/querier/compare/v1.6.1...v1.6.2) (2024-09-06)


### Bug Fixes

* **workflow:** update release workflow ([640c835](https://github.com/villagers/querier/commit/640c835e45d5d615e58a9485975c29533af01ba1))

# [1.5.0](https://github.com/villagers/querier/compare/v1.4.2...v1.5.0) (2024-09-05)


### Features

* add `columnAs` parameter for date functions ([0d249e2](https://github.com/villagers/querier/commit/0d249e21e8eac049f8effc321c820187d890af3a))
