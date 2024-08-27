# Contributing to [Your Project Name]

Thank you for considering contributing to [Your Project Name]! We're excited to have you collaborate with us. Below are guidelines to help you contribute effectively.

## Table of Contents
1. [How to Contribute](#how-to-contribute)
2. [Branching Strategy](#branching-strategy)
3. [Submitting Changes](#submitting-changes)
4. [Style Guides](#style-guides)

## How to Contribute

1. **Fork the repository** to your own GitHub account.
2. **Clone the project** to your local machine.
3. **Create a new branch** following our [Branching Strategy](#branching-strategy).
4. **Make your changes**.
5. **Test your changes** thoroughly.
6. **Submit a pull request** with a clear description of your changes.

## Branching Strategy

We use a feature branching strategy to keep our codebase clean and organized. Here's how it works:

- **master**: The master branch contains the latest version of code that is released to production.
- **develop**: The develop branch contains the latest development changes and is the default branch for merging 'dev' branches.
- **feature/{issue}**: New feature work
- **bug/{issue}**: A bug fix
- **hotfix/{issue}**: A fix to critical bugs in production
- **release/{issue}**: To prepare a new release
- **docs/{issue}**: Documentation additions or changes

Note: '{issue}' means github issue in the haul-this project. All dev branches should link to a issue in their name.

### Creating a New Dev Branch

1. Make sure your local develop branch is up-to-date:
    ```sh
    git checkout develop
    git pull origin develop
    ```
2. Create a new branch from develop:
    ```sh
    git checkout -b prefix/{issue}
    ```

### Merging a Dev Branch

1. Ensure your dev branch is up-to-date with develop:
    ```sh
    git checkout develop
    git pull origin develop
    git checkout prefix/{issue}
    git rebase develop
    ```
2. Once your feature is complete and tested, open a pull request to merge your dev branch into develop. **Ensure that your pull request includes a descriptive name linking back to the issue it implements, and include a description containing a summary of changes**
3. After review and approval, your changes will be merged into develop.

## Submitting Changes

1. Ensure your changes follow the project's style guides.
2. Write clear, concise commit messages.
3. Push your feature branch to your forked repository:
    ```sh
    git push origin prefix/{issue}
    ```
4. Open a pull request from your feature branch to the develop branch.
5. Provide a detailed description of the changes and any related issue numbers.

## Style Guides

- **Code Style**: This projects code follows [Google's C# Coding Style](https://google.github.io/styleguide/csharp-style.html).
- **Commit Messages**: Use clear and concise commit messages. Follow the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) specification.
- **Documentation**: Update documentation for any new features or changes in behavior.

