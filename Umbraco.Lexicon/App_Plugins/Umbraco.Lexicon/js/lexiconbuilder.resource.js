function lexiconBuilderResource($q, $http, umbRequestHelper) {

    return {
        buildLexicon: function () {
            return umbRequestHelper.resourcePromise(
                $http.post("/umbraco/backoffice/api/lexicon/BuildLexicon"), "Failed to build Lexicon");
        }
    };
}
angular.module("umbraco.resources").factory("lexiconBuilderResource", lexiconBuilderResource);