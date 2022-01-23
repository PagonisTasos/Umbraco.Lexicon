function lexiconBuilderController(lexiconBuilderResource, notificationsService) {

    var vm = this;

    vm.generate = generate;

    function generate() {
        lexiconBuilderResource.buildLexicon().then(
            (result) => notificationsService.success("Lexicon generated", "Lexicon.generated.cs has been overridden"),
            () => notificationsService.error("Generation Failed", "Generation is only available in debug mode. Check the logs for more info.")
        );
    }
}

angular.module("umbraco").controller("LexiconBuilderController", lexiconBuilderController);