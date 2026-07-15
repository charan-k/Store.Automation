pipeline {
    agent any

    parameters {
        choice(
            name: 'BROWSER',
            choices: ['chrome', 'firefox'],
            description: 'Browser to use for UI Tests'
        )
        booleanParam(
            name: 'RUN_UI_TESTS',
            defaultValue: true,
            description: 'Run UI Tests'
        )
        booleanParam(
            name: 'RUN_API_TESTS',
            defaultValue: true,
            description: 'Run API Tests'
        )
        booleanParam(
            name: 'HEADLESS',
            defaultValue: true,
            description: 'Run browser in headless mode (UI Tests only)'
        )
    }

    environment {
        SOLUTION_PATH     = 'Store.Automation.slnx'
        TEST_PROJECT_PATH = 'BookStore.Tests/Store.Tests.csproj'
        RUNSETTINGS_PATH  = 'BookStore/nunit.runsettings'
        RESULTS_DIR       = 'TestResults'
        LOGS_DIR          = 'Logs'
        SCREENSHOTS_DIR   = 'Screenshots'
    }

    stages {

        stage('Restore Packages') {
            steps {
                echo '📦 Restoring NuGet packages...'
                bat 'dotnet restore %SOLUTION_PATH%'
            }
        }

        stage('Build Solution') {
            steps {
                echo '🔨 Building the solution...'
                bat 'dotnet build %SOLUTION_PATH% --configuration Release --no-restore'
            }
        }

        stage('Run API Tests') {
            when {
                expression { return params.RUN_API_TESTS == true }
            }
            steps {
                echo '🔗 Running API Tests...'
                bat """
                    dotnet test %TEST_PROJECT_PATH% ^
                        --configuration Release ^
                        --no-build ^
                        --filter "Category=API" ^
                        --settings %RUNSETTINGS_PATH% ^
                        --logger "trx;LogFileName=api-test-results.trx" ^
                        --results-directory %RESULTS_DIR%
                """
            }
            post {
                always {
                    echo '📊 Archiving API test results...'
                    junit '**/TestResults/api-test-results.trx'
                    archiveArtifacts(
                        artifacts: '**/TestResults/api-test-results.trx',
                        allowEmptyArchive: true
                    )
                }
                success {
                    echo '✅ API Tests passed!'
                }
                failure {
                    echo '❌ API Tests failed! Check the test results.'
                }
            }
        }

        stage('Run UI Tests') {
            when {
                expression { return params.RUN_UI_TESTS == true }
            }
            environment {
                BrowserType = "${params.BROWSER}"
                Headless    = "${params.HEADLESS}"
            }
            steps {
                echo '🌐 Running UI Tests...'
                bat """
                    dotnet test %TEST_PROJECT_PATH% ^
                        --configuration Release ^
                        --no-build ^
                        --filter "Category=UI" ^
                        --settings %RUNSETTINGS_PATH% ^
                        --logger "trx;LogFileName=ui-test-results.trx" ^
                        --results-directory %RESULTS_DIR%
                """
            }
            post {
                always {
                    echo '📊 Archiving UI test results...'
                    junit '**/TestResults/ui-test-results.trx'
                    archiveArtifacts(
                        artifacts: '**/TestResults/ui-test-results.trx',
                        allowEmptyArchive: true
                    )
                    archiveArtifacts(
                        artifacts: '**/Screenshots/**',
                        allowEmptyArchive: true
                    )
                }
                success {
                    echo '✅ UI Tests passed!'
                }
                failure {
                    echo '❌ UI Tests failed! Check screenshots and logs.'
                }
            }
        }

        stage('Publish Logs') {
            steps {
                echo '📋 Publishing logs...'
                archiveArtifacts(
                    artifacts: '**/Logs/**',
                    allowEmptyArchive: true
                )
            }
        }

        stage('Publish Test Reports') {
            steps {
                echo '📈 Publishing test reports...'
                publishHTML(target: [
                    allowMissing         : true,
                    alwaysLinkToLastBuild: true,
                    keepAll              : true,
                    reportDir            : "${RESULTS_DIR}",
                    reportFiles          : '*.trx',
                    reportName           : 'BookStore Automation Test Results'
                ])
            }
        }
    }

    post {
        always {
            echo '🧹 Cleaning workspace...'
            cleanWs()
        }
        success {
            echo '🎉 Pipeline completed! All selected tests passed.'
        }
        failure {
            echo '🚨 Pipeline failed! Check test results, logs and screenshots.'
        }
        unstable {
            echo '⚠️ Pipeline unstable! Some tests may have failed.'
        }
    }
}