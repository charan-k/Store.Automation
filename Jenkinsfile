pipeline {
    agent any

    // ✅ Parameters - Configurable from Jenkins UI
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

    // ✅ Global Environment Variables
    environment {
        SOLUTION_PATH        = 'YourSolution.sln'
        TEST_PROJECT_PATH    = 'YourTestProject/YourTestProject.csproj'
        RESULTS_DIR          = 'TestResults'
        LOGS_DIR             = 'Logs'
        SCREENSHOTS_DIR      = 'Screenshots'
    }

    stages {

        // ✅ Stage 1 - Checkout Code
        stage('Checkout Code') {
            steps {
                echo '📥 Checking out source code...'
                checkout scm
            }
        }

        // ✅ Stage 2 - Restore NuGet Packages
        stage('Restore Packages') {
            steps {
                echo '📦 Restoring NuGet packages...'
                sh 'dotnet restore ${SOLUTION_PATH}'
            }
        }

        // ✅ Stage 3 - Build Solution
        stage('Build Solution') {
            steps {
                echo '🔨 Building the solution...'
                sh 'dotnet build ${SOLUTION_PATH} --configuration Release --no-restore'
            }
        }

        // ✅ Stage 4 - Run API Tests
        stage('Run API Tests') {
            when {
                expression { return params.RUN_API_TESTS == true }
            }
            environment {
                // API Tests only need BaseAPIUrl
                // No browser or headless needed
                ENVIRONMENT  = "${params.ENVIRONMENT}"
            }
            steps {
                echo '🔗 Running API Tests...'
                sh """
                    dotnet test ${TEST_PROJECT_PATH} \
                        --configuration Release \
                        --no-build \
                        --filter "Category=API" \
                        --logger "trx;LogFileName=api-test-results.trx" \
                        --results-directory ${RESULTS_DIR}
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

        // ✅ Stage 5 - Run UI Tests
        stage('Run UI Tests') {
            when {
                expression { return params.RUN_UI_TESTS == true }
            }
            environment {
                // ✅ UI Tests need Browser, Headless and Environment
                // Config.cs reads these environment variables
                BrowserType  = "${params.BROWSER}"
                Headless     = "${params.HEADLESS}"
                ENVIRONMENT  = "${params.ENVIRONMENT}"
                DISPLAY      = ':99' // Required for Linux Jenkins agents (no GUI)
            }
            steps {
                echo '🌐 Running UI Tests...'
                sh """
                    dotnet test ${TEST_PROJECT_PATH} \
                        --configuration Release \
                        --no-build \
                        --filter "Category=UI" \
                        --logger "trx;LogFileName=ui-test-results.trx" \
                        --results-directory ${RESULTS_DIR}
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
                    // ✅ Archive screenshots captured on test failure
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

        // ✅ Stage 6 - Publish Logs
        stage('Publish Logs') {
            steps {
                echo '📋 Publishing logs...'
                archiveArtifacts(
                    artifacts: '**/Logs/**',
                    allowEmptyArchive: true
                )
            }
        }

        // ✅ Stage 7 - Publish Test Reports
        stage('Publish Test Reports') {
            steps {
                echo '📈 Publishing test reports...'
                publishHTML(target: [
                    allowMissing         : true,
                    alwaysLinkToLastBuild: true,
                    keepAll              : true,
                    reportDir            : "${RESULTS_DIR}",
                    reportFiles          : '*.trx',
                    reportName           : 'Automation Test Results'
                ])
            }
        }
    }

    // ✅ Global Post Actions
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