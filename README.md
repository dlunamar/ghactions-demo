# GhActionsDemo

Proyecto de práctica de **GitHub Actions**: una librería .NET 10 con un calculador
de bandas de riesgo (misma idea que el cuantificador de `Conversia.GeneradorDoc`)
y dos workflows reales de CI/CD.

```
ghactions-demo/
├── .github/workflows/
│   ├── ci.yml          # build + tests en cada PR y push a main
│   └── release.yml     # publish + GitHub Release al etiquetar (v*)
├── src/GhActionsDemo.Core/        # dominio (RiskBandCalculator)
├── tests/GhActionsDemo.Core.Tests # xUnit: 12 tests (casos en borde + excepciones)
├── Directory.Build.props          # RestorePackagesWithLockFile (caché CI + reproducibilidad)
└── GhActionsDemo.slnx
```

## Cómo lanzarlo en local

```powershell
dotnet test GhActionsDemo.slnx
```

## Cómo publicarlo en GitHub

```powershell
git init
git add .
git commit -m "feat: demo de GitHub Actions (CI + release)"
git branch -M main
git remote add origin https://github.com/<tu-usuario>/ghactions-demo.git
git push -u origin main
```

Los workflows se ejecutan solos al abrir un PR o hacer push a `main`
(pestaña **Actions** del repo).

Para las **aprobaciones de producción**: Settings → Environments → `production`
→ *Required reviewers*. Ahí también van los secretos del entorno
(`Settings → Environments → production → Environment secrets`), nunca en el YAML.

## Equivalencia con Azure DevOps (para la entrevista)

| GitHub Actions | Azure DevOps Pipelines |
|---|---|
| `on: push / pull_request` | `trigger` / `pr:` |
| `jobs.<job>.runs-on` | `pool: vmImage` |
| `steps` + `uses:` | `steps` + `tasks@` / `script` |
| `strategy.matrix` | `strategy.matrix` |
| `secrets` / `environment` | variable groups + Key Vault |
| `environment: production` + reviewers | stage con `environment` + aprovals |
| `actions/upload-artifact` | `PublishBuildArtifacts@1` |
| `concurrency.cancel-in-progress` | cancel previous runs |
| `permissions:` (mínimo privilegio) | `variables:` / task permissions |

## Ideas clave que cubre

- **CI en PR**: restore → build → test → subir `.trx` aunque fallen (`if: always()`).
- **Caché de NuGet** por hash de `packages.lock.json`.
- **Matriz** ubuntu + windows.
- **Release por tag** con `environment` (aprobaciones y secretos por entorno).
- **Mínimo privilegio** con `permissions:` a nivel de workflow.
- **Rollback**: volver a publicar el tag anterior (el artefacto de cada release queda).
