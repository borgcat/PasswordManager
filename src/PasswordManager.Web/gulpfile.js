/// <binding Clean='clean, min' />

var gulp = require("gulp");
var rimraf = require("rimraf");
var concat = require("gulp-concat");
var cssmin = require("gulp-cssmin");
var uglify = require("gulp-uglify");
var gutil = require('gulp-util');
var gulpif = require('gulp-if');
var autoprefixer = require('gulp-autoprefixer');
var less = require('gulp-less');
var plumber = require('gulp-plumber');
var buffer = require('vinyl-buffer');
var source = require('vinyl-source-stream');
var babelify = require('babelify');
var browserify = require('browserify');
var watchify = require('watchify');
var sourcemaps = require('gulp-sourcemaps');
var lint = require('gulp-eslint');
var project = require("./project.json");

//var htmlreplace = require('gulp-html-replace');
//var reactify = require('reactify');
//var streamify = require('gulp-streamify');

var paths = {
    webroot: "./" + project.webroot + "/"
};

gulp.task("clean:js", function (cb) {
    rimraf(paths.webroot + 'js/vendor.js');
    rimraf(paths.webroot + 'js/vendor.bundle.js', cb);
    //rimraf(paths.webroot + 'js/bundle.js', cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.webroot + 'css/**/*.css', cb);
});

gulp.task("clean", ['clean:js', 'clean:css']);

var dependencies = [
    'alt',
    'react',
    'react-dom',
    'react-router',
    'underscore'
];

gulp.task('vendor', function () {
    return gulp.src([
        paths.webroot + 'lib/jquery/dist/jquery.js',
        paths.webroot + 'lib/bootstrap/dist/js/bootstrap.js',
        paths.webroot + 'lib/magnific-popup/dist/jquery.magnific-popup.js',
        paths.webroot + 'lib/toastr/toastr.js'
    ])
        .pipe(concat('vendor.js'))
        .pipe(uglify({ mangle: false }))
        .pipe(gulp.dest(paths.webroot + '/js'));
});

gulp.task('browserify-vendor', function () {
    return browserify()
        .require(dependencies)
        .bundle()
        .pipe(source('vendor.bundle.js'))
        .pipe(buffer())
        .pipe(uglify({ mangle: false }))
        .pipe(gulp.dest(paths.webroot + '/js'));
});

gulp.task('browserify', ['browserify-vendor'], function () {
    return browserify({ entries: './Client/app/main.js', debug: true })
        //.external(dependencies)
        .transform(babelify, { presets: ['es2015', 'react'] })
        .bundle()
        .pipe(source('bundle.js'))
        .pipe(buffer())
        .pipe(sourcemaps.init({ loadMaps: true }))
        .pipe(uglify({ mangle: false }))
        .pipe(sourcemaps.write('.'))
        .pipe(gulp.dest(paths.webroot + '/js'));
});

gulp.task('browserify-watch', ['browserify-vendor'], function () {
    var bundler = watchify(browserify({ entries: 'Client/app/main.js', debug: true }, watchify.args));
    bundler.external(dependencies);
    bundler.transform(babelify, { presets: ['es2015', 'react'] });
    bundler.on('update', rebundle);
    return rebundle();

    function rebundle() {
        var start = Date.now();
        return bundler.bundle()
            .on('error', function (err) {
                gutil.log(gutil.colors.red(err.toString()));
            })
            .on('end', function () {
                gutil.log(gutil.colors.green('Finished rebundling in', (Date.now() - start) + 'ms.'));
            })
            .pipe(source('bundle.js'))
            .pipe(buffer())
            .pipe(sourcemaps.init({ loadMaps: true }))
            .pipe(sourcemaps.write('.'))
            .pipe(gulp.dest(paths.webroot + '/js'));
    }
});

//gulp.task('transform', function () {
//    gulp.src(path.JS)
//      .pipe(react())
//      .pipe(gulp.dest(path.DEST_SRC));
//});

//still working on theses to port from node to .NET core
gulp.task('styles', function () {
    return gulp.src('Client/app/stylesheets/main.less')
        .pipe(plumber())
        .pipe(less())
        .pipe(autoprefixer())
        .pipe(cssmin())
        .pipe(gulp.dest(paths.webroot + '/css'));
});

gulp.task('lint', function () {
    return gulp.src('bundle.js')
        .pipe(lint({ config: 'eslint.config.json' }))
        .pipe(lint.format());
});


gulp.task('watch', function () {
    gulp.watch('Client/app/stylesheets/**/*.less', ['styles']);
});

gulp.task('default', ['styles', 'vendor', 'browserify-watch', 'lint', 'watch']);
gulp.task('build', ['styles', 'vendor', 'browserify']);
