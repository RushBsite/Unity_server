PROJECT_PATH="$GAME_SERVER_PROJECT_PATH"
GAME_SERVER_PATH=\\Server\\bin\\Release\\netcoreapp3.1
FULL_BUILD_PATH="$PROJECT_PATH""$GAME_SERVER_PATH"

OS=AMAZON_LINUX
BUILD_NAME=PLServer
BUILD_VERSION=0.0.1
REGION=ap-northeast-2

aws gamelift upload-build \
--operating-system "$OS" \
--build-root "$FULL_BUILD_PATH" \
--name "$BUILD_NAME" \
--build-version "$BUILD_VERSION" \
--region "$REGION"