module.exports = {
    branches: ["main"],
    plugins: [
        "@semantic-release/commit-analyzer",
        [
            "@semantic-release/release-notes-generator",
            {
                preset: "conventionalcommits",
                presetConfig: {
                    types: [
                        { type: "feat", section: "Features" },
                        { type: "fix", section: "Bug Fixes" },
                        { type: "perf", section: "Performance Improvements" },
                        { type: "revert", section: "Reverts" },
                        { type: "docs", section: "Documentation" },
                        { type: "style", section: "Styles" },
                        { type: "chore", section: "Miscellaneous Chores" },
                        { type: "refactor", section: "Code Refactoring" },
                        { type: "test", section: "Tests" },
                        { type: "build", section: "Build System" },
                        { type: "ci", section: "Continuous Integration" },
                        { type: "improvement", section: "Improvement" },
                    ],
                },
            },
        ],
        [
            "@semantic-release/changelog",
            {
                changelogFile: "CHANGELOG.md",
            },
        ],
        [
            "@semantic-release/exec",
            {
                prepareCmd: "pwsh -NoLogo -NoProfile -NonInteractive -Command ./prepare.ps1 '${ nextRelease.version }' '${ nextRelease.gitHead }' '${ options.repositoryUrl }' '${process.env.CSPROJ}'",
            }
        ],
        [
        "@semantic-release/git",
        {
            assets: ["CHANGELOG.md", "package.json", "package-lock.json"],
        },
        ],
        "@semantic-release/npm",
        "@semantic-release/github",
    ],

}