        var noInputValue = 0.000;

        jQuery("#calculate").click(function() {

            function arc(radius, degree) {
                var arcVar = radius * degree * .01745;
                return arcVar;
            }

            var radius = jQuery("#radius").val();
            var degree = jQuery("#degree").val();
            var arcv = arc(radius, degree);
            arcv = arcv.toFixed(3);
            jQuery("#arcTotal").text(arcv);

            return false;
        });

        jQuery("#ovalityCalculate").click(function() {

            var minOD = jQuery("#ovalityMinOD").val();
            var maxOD = jQuery("#ovalityMaxOD").val();
            var nominalOD = jQuery("#ovalityNominalOD").val();

            var ovalityTotal = (maxOD - minOD) / nominalOD * 100;
            ovalityTotal = ovalityTotal.toFixed(3);

            jQuery("#ovalityTotal").text(ovalityTotal);

            return false;
        });

        jQuery("#radCalculate").click(function() {

            function rad(chord, rise) {
                var rSqr = Math.pow(rise, 2);
                var half = chord / 2;
                var cSqr = Math.pow(half, 2);
                var result = (rSqr + cSqr) / (2 * rise);
                return result;
            }

            var chord = jQuery("#chord").val();
            var rise = jQuery("#rise").val();
            var result = rad(chord, rise);
            if (result) {
                result = result.toFixed(3);
                jQuery("#radTotalOne").text(result);
            } else {
                result = noInputValue.toFixed(3);
                jQuery("#radTotalOne").text(result);
            }
            return false;
        });

        jQuery("#radCalculateB").click(function() {

            function rad(arc, deg) {
                var result = arc / (.01745 * deg);
                return result;
            }

            var arc = jQuery("#arcLength").val();
            var deg = jQuery("#degreeBend").val();
            var result = rad(arc, deg);
            if (result) {
                result = result.toFixed(3);
                jQuery("#radTotal").text(result);
            } else {
                result = noInputValue.toFixed(3);
                jQuery("#radTotal").text(result);
            }
            return false;
        });


        jQuery("#moCalculate").click(function() {

            function calcRise(radius, chord) {
                var chr = chord / 2;
                var sqrR = Math.pow(radius, 2);
                var sqrC = Math.pow(chr, 2);
                var root = Math.sqrt(sqrR - sqrC);
                var result = radius - root;
                return result;
            }

            var radius = jQuery("#moRadius").val();
            var chord = jQuery("#moChord").val();
            var result = calcRise(radius, chord);
            result = result.toFixed(3);
            jQuery("#moTotal").text(result);
            return false;
        });

        jQuery("#degCalculate").click(function() {

            function calcDegree(length, radius) {
                var degree = length / (.01745 * radius);
                return degree;
            }

            var length = jQuery("#degLength").val();
            var radius = jQuery("#degRadius").val();
            var result = calcDegree(length, radius);
            if (result) {
                result = result.toFixed(3);
                jQuery("#degTotal").text(result);
            } else {
                result = noInputValue.toFixed(3);
                jQuery("#degTotal").text(result);
            }
            return false;
        });

        var clear = $(".clear");
        clear.click(function(obj) {
            var form = $(this).parent().parent(".calculator");
            var inputs = form.find("input.form-control");
            inputs.val("");
            var results = form.find(".result");
            results.html("0.000");
        });



