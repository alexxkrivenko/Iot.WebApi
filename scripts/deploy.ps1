# https://www.appveyor.com/docs/environment-variables/

# Script static variables
$projectDir = $buildDir + "\";

# Build docker image
Write-Host "Build docker image" -ForegroundColor Green
docker build -t $env:DOCKER_USER/$env:DOCKER_IMAGE_NAME -f Dockerfile --build-arg acrh=amd64 .

# Push docker image
Write-Host "Push docker image" -ForegroundColor Green
docker login -u="$env:DOCKER_USER" -p="$env:DOCKER_PASS"
docker push $env:DOCKER_USER/$env:DOCKER_IMAGE_NAME

# Done
Write-Host "Done!" -ForegroundColor Green